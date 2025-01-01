import { useState } from "react";
import { useNavigate } from "react-router";
import { UseAuth } from "./context/AuthContext";
import { BASE_URL, LOGIN_PATH } from "../API/URLs";

interface LoginForm {
	email: string;
	password: string;
}

interface LoginResponse {
	userId: string;
	accessToken: string;
	accessTokenExpirationDate: string;
	refreshToken: string;
	refreshTokenExpirationDate: string;
}

const LoginCall = async (formData: LoginForm) => {
	return fetch(`${BASE_URL}/${LOGIN_PATH}`, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(formData),
	});
};

export default function Login() {
	const navigate = useNavigate();
	const { login } = UseAuth();
	const [showPassword, setShowPassword] = useState(false);
	const [formData, setFormData] = useState<LoginForm>({
		email: "",
		password: "",
	});
	const [error, setError] = useState<string>("");

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		setError("");

		try {
			const response = await LoginCall(formData);
			if (!response.ok) throw new Error("Login failed");

			const data: LoginResponse = await response.json();

			login(data.accessToken);
			localStorage.setItem("refreshToken", data.refreshToken);
			localStorage.setItem("userId", data.userId);

			console.log("Login successful: about to navigate to /dashboard");
			navigate("/", { replace: true });
		} catch (err: Error | unknown) {
			setError("Invalid email or password");
			console.error(err);
		}
	};

	return (
		<div className="container mt-5">
			<div className="row justify-content-center">
				<div className="col-md-6">
					<div className="card">
						<div className="card-body">
							<h2 className="text-center mb-4">Login</h2>
							{error && (
								<div className="alert alert-danger">
									{error}
								</div>
							)}
							<form onSubmit={handleSubmit}>
								<div className="mb-3">
									<label
										htmlFor="email"
										className="form-label"
									>
										Email:
									</label>
									<input
										type="email"
										id="email"
										placeholder="your email"
										className="form-control"
										value={formData.email}
										onChange={(e) =>
											setFormData({
												...formData,
												email: e.target.value,
											})
										}
										required
									/>
								</div>
								<div className="mb-3">
									<label
										htmlFor="password"
										className="form-label"
									>
										Password:
									</label>
									<div className="input-group">
										<input
											type={
												showPassword
													? "text"
													: "password"
											}
											id="password"
											placeholder="your password"
											className="form-control"
											value={formData.password}
											onChange={(e) =>
												setFormData({
													...formData,
													password: e.target.value,
												})
											}
											required
										/>
										<button
											className="btn btn-outline-secondary"
											type="button"
											onClick={() =>
												setShowPassword(!showPassword)
											}
										>
											<i
												className={`bi ${
													showPassword
														? "bi-eye-slash"
														: "bi-eye"
												}`}
											></i>
										</button>
									</div>
								</div>
								<button
									type="submit"
									className="btn btn-primary w-100"
								>
									Login
								</button>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
