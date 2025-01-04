import { useState } from "react";
import { useNavigate } from "react-router";
import { UseAuth } from "./context/AuthContext";
import { RegisterRequest } from "../API/interfaces";
import { authApi } from "../API/Calls/AuthCalls";

export default function Register() {
	const navigate = useNavigate();
	const { login } = UseAuth();
	const [formData, setFormData] = useState<RegisterRequest>({
		email: "",
		username: "",
		firstName: "",
		lastName: "",
		password: "",
	});
	const [confirmPassword, setConfirmPassword] = useState<string>("");
	const [error, setError] = useState<string>("");
	const [result, setResult] = useState<string>("");
	const [showPassword, setShowPassword] = useState(false);
	const [showConfirmPassword, setShowConfirmPassword] = useState(false);

	const handleSubmit = async (e: React.FormEvent) => {
		try {
			e.preventDefault();
			setError("");
			if (formData.password !== confirmPassword) {
				setError("Passwords do not match");
				return;
			}
			const response = await authApi.register(formData);
			login(response);

			setResult("Successfully registered as " + formData.email);
			await new Promise((resolve) => setTimeout(resolve, 2000));

			navigate("/", { replace: true });
		} catch (err: Error | unknown) {
			setError(
				err instanceof Error ? err.message : "Registration failed"
			);
			console.error(err);
		}
	};

	return (
		<div className="container mt-5">
			<div className="row justify-content-center">
				<div className="col-md-6">
					<div className="card">
						<div className="card-body">
							<h2 className="text-center mb-4">Register</h2>
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
										className="form-control"
										placeholder="user@example.com"
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
										htmlFor="username"
										className="form-label"
									>
										Username:
									</label>
									<input
										type="text"
										id="username"
										placeholder="username"
										className="form-control"
										value={formData.username}
										onChange={(e) =>
											setFormData({
												...formData,
												username: e.target.value,
											})
										}
										required
									/>
								</div>
								<div className="mb-3">
									<label
										htmlFor="firstName"
										className="form-label"
									>
										First Name:
									</label>
									<input
										type="text"
										id="firstName"
										placeholder="First Name"
										className="form-control"
										value={formData.firstName}
										onChange={(e) =>
											setFormData({
												...formData,
												firstName: e.target.value,
											})
										}
										required
										minLength={3}
										maxLength={25}
									/>
								</div>
								<div className="mb-3">
									<label
										htmlFor="lastName"
										className="form-label"
									>
										Last Name:
									</label>
									<input
										type="text"
										id="lastName"
										placeholder="Last Name"
										className="form-control"
										value={formData.lastName}
										onChange={(e) =>
											setFormData({
												...formData,
												lastName: e.target.value,
											})
										}
										required
										minLength={3}
										maxLength={25}
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
											placeholder="Strong Password"
											className="form-control"
											value={formData.password}
											onChange={(e) =>
												setFormData({
													...formData,
													password: e.target.value,
												})
											}
											required
											minLength={12}
											maxLength={100}
											pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':|\,.<>/?]).{12,100}$"
											title="Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character"
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

								<div className="mb-3">
									<label
										htmlFor="confirmPassword"
										className="form-label"
									>
										Confirm Password:
									</label>
									<div className="input-group">
										<input
											type={
												showConfirmPassword
													? "text"
													: "password"
											}
											id="confirmPassword"
											className="form-control"
											value={confirmPassword}
											onChange={(e) =>
												setConfirmPassword(
													e.target.value
												)
											}
											required
											placeholder="Retype Password"
										/>
										<button
											className="btn btn-outline-secondary"
											type="button"
											onClick={() =>
												setShowConfirmPassword(
													!showConfirmPassword
												)
											}
										>
											<i
												className={`bi ${
													showConfirmPassword
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
									Register
								</button>
							</form>
							<div className="have-account mt-2">
								Do you have account?{" "}
								<span
									className="login-link text-primary cursor-pointer"
									onClick={() =>
										navigate("/login", {
											replace: true,
										})
									}
								>
									Login Now
								</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}
