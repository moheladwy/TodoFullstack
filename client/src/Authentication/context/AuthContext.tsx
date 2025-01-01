import { createContext, useContext, useState, useEffect } from "react";
import { BASE_URL, REFRESH_PATH } from "../../API/URLs";

interface AuthContextType {
	isAuthenticated: boolean;
	accessToken: string | null;
	userId: string | null;
	isSidebarOpen: boolean;
	toggleSidebar: () => void;
	login: (token: string) => void;
	logout: () => void;
	refreshAccessToken: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | null>(null);

const RefreshToken = async (refreshToken: string) => {
	return await fetch(`${BASE_URL}/${REFRESH_PATH}`, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(refreshToken),
	});
};

function AuthProvider({ children }: { children: React.ReactNode }) {
	const [isAuthenticated, setIsAuthenticated] = useState(false);
	const [accessToken, setAccessToken] = useState<string | null>(null);
	const [userId, setUserId] = useState<string | null>(null);
	const [isLoading, setIsLoading] = useState(true);
	const [isSidebarOpen, setIsSidebarOpen] = useState(() => {
		const saved = localStorage.getItem("sidebarOpen");
		return saved !== null ? JSON.parse(saved) : true;
	});

	useEffect(() => {
		const initializeAuth = async () => {
			const token = sessionStorage.getItem("accessToken");
			const refreshToken = localStorage.getItem("refreshToken");
			if (token) {
				setAccessToken(token);
				setIsAuthenticated(true);
				setUserId(localStorage.getItem("userId"));
			} else if (refreshToken) {
				try {
					const response = await RefreshToken(refreshToken);
					if (!response.ok) throw new Error("Token refresh failed");

					const data = await response.json();
					login(data.accessToken);
					localStorage.setItem("refreshToken", data.refreshToken);
					setUserId(data.userId);
				} catch (err: Error | unknown) {
					console.error(err);
					logout();
				}
			}
			setIsLoading(false);
		};
		initializeAuth();
	}, []);

	const login = (token: string) => {
		sessionStorage.setItem("accessToken", token);
		setAccessToken(token);
		setIsAuthenticated(true);
	};

	const logout = () => {
		sessionStorage.removeItem("accessToken");
		localStorage.removeItem("refreshToken");
		localStorage.removeItem("userId");
		setAccessToken(null);
		setIsAuthenticated(false);
	};

	const refreshAccessToken = async () => {
		const refreshToken = localStorage.getItem("refreshToken");
		if (!refreshToken) {
			logout();
			return;
		}

		try {
			const response = await RefreshToken(refreshToken);
			if (!response.ok) throw new Error("Token refresh failed");

			const data = await response.json();
			login(data.accessToken);
			localStorage.setItem("refreshToken", data.refreshToken);
			localStorage.setItem("userId", data.userId);
		} catch (err: Error | unknown) {
			logout();
			console.error(err);
		}
	};

	const toggleSidebar = () => {
		const newState = !isSidebarOpen;
		setIsSidebarOpen(newState);
		localStorage.setItem("sidebarOpen", JSON.stringify(newState));
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
				isSidebarOpen,
				login,
				logout,
				refreshAccessToken,
				toggleSidebar,
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

export { AuthContext, AuthProvider, UseAuth };
