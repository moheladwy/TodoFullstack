import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { UseAuth } from "./context/AuthContext";
import { LoginRequest } from "../API/interfaces";
import { authApi } from "../API/Calls/AuthCalls";

export default function Login() {
	const navigate = useNavigate();
	const { login } = UseAuth();
	const [showPassword, setShowPassword] = useState(false);
	const [formData, setFormData] = useState<LoginRequest>({
		email: "",
		password: "",
	});
	const [error, setError] = useState<string>("");
	const [result, setResult] = useState<string>("");

	useEffect(() => {
		setShowPassword(false);
		setFormData({
			email: "",
			password: "",
		});
		setError("");
		setResult("");
	}, []);

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		setError("");

		try {
			const response = await authApi.login(formData);
			login(response);

			setResult("Successfully logged in as " + formData.email);
			await new Promise((resolve) => setTimeout(resolve, 2000));

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
							{result && (
								<div className="alert alert-success">
									{result}
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
							<div className="have-no-account mt-2">
								Don't have an account?{" "}
								<span
									className="register-link text-primary cursor-pointer"
									onClick={() =>
										navigate("/register", {
											replace: true,
										})
									}
								>
									Register Now
								</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
