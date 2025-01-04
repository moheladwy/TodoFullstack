import { createContext, useContext, useState, useEffect } from "react";
import { authApi } from "../../API/Calls/AuthCalls";
import { AuthResponse, User } from "../../API/interfaces";
import { userApi } from "../../API/Calls/UserCalls";

interface AuthContextType {
	isAuthenticated: boolean;
	accessToken: string | null;
	userId: string | null;
	user: User | null;
	login: (authResponse: AuthResponse) => void;
	logout: () => void;
	refreshAccessToken: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

function AuthProvider({ children }: { children: React.ReactNode }) {
	const [isAuthenticated, setIsAuthenticated] = useState(false);
	const [accessToken, setAccessToken] = useState<string | null>(null);
	const [userId, setUserId] = useState<string | null>(null);
	const [user, setUser] = useState<User | null>(null);
	const [isLoading, setIsLoading] = useState(true);

	useEffect(() => {
		const initializeAuth = async () => {
			const token = localStorage.getItem("accessToken");
			const refreshToken = localStorage.getItem("refreshToken");
			const userId = localStorage.getItem("userId");
			if (token) {
				setAccessToken(token);
				setIsAuthenticated(true);
				setUserId(userId);
			} else if (refreshToken) {
				try {
					const authResponse = await authApi.refresh(refreshToken);
					login(authResponse);
				} catch (err: Error | unknown) {
					console.error(err);
					logout();
				}
			}
			setIsLoading(false);
		};
		initializeAuth();
	}, []);

	const login = async (authResponse: AuthResponse) => {
		localStorage.setItem("accessToken", authResponse.accessToken);
		localStorage.setItem("refreshToken", authResponse.refreshToken);
		localStorage.setItem("userId", authResponse.userId);
		setAccessToken(authResponse.accessToken);
		setUserId(authResponse.userId);
		setIsAuthenticated(true);
		setUser(await userApi.getUser(authResponse.userId));
	};

	const logout = async () => {
		try {
			await authApi.logout();
		} catch (error: Error | unknown) {
			console.error(error);
		}
		localStorage.removeItem("accessToken");
		localStorage.removeItem("refreshToken");
		localStorage.removeItem("userId");
		setAccessToken(null);
		setUserId(null);
		setIsAuthenticated(false);
	};

	const refreshAccessToken = async () => {
		try {
			const refreshToken = localStorage.getItem("refreshToken");
			if (!refreshToken) {
				logout();
				return;
			}
			const response = await authApi.refresh(refreshToken);
			login(response);
		} catch (err: Error | unknown) {
			logout();
			console.error(err);
		}
	};

	if (isLoading) {
		return <div>Loading...</div>;
	}
	return (
		<AuthContext.Provider
			value={{
				isAuthenticated,
				accessToken,
				userId,
				user,
				login,
				logout,
				refreshAccessToken,
			}}
		>
			{children}
		</AuthContext.Provider>
	);
}

const UseAuth = () => {
	const context = useContext(AuthContext);
	if (!context) {
		throw new Error("useAuth must be used within an AuthProvider");
	}
	return context;
};

export { AuthProvider, UseAuth };
