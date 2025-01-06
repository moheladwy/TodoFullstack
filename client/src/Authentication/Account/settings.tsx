import { useEffect, useState } from "react";
import { UseAuth } from "../context/AuthContext";
import { userApi } from "../../API/Calls/UserCalls";
import {
	UpdateUserInfoRequest,
	ChangePasswordRequest,
} from "../../API/interfaces";

export default function AccountSettings() {
	const { user, updateUser, logout } = UseAuth();
	const [userNewInfo, setUserInfo] = useState<UpdateUserInfoRequest>({
		id: user?.id || "",
		newFirstName: user?.firstName || "",
		newLastName: user?.lastName || "",
		newEmail: user?.email || "",
		newUsername: user?.userName || "",
		newPhoneNumber: user?.phoneNumber || "",
	});
	const [confirmPassword, setConfirmPassword] = useState<string>("");
	const [passwordInfo, setPasswordInfo] = useState<ChangePasswordRequest>({
		id: user?.id || "",
		currentPassword: "",
		newPassword: "",
	});
	const [passwordVisibility, setPasswordVisibility] = useState({
		currentPassword: false,
		newPassword: false,
		confirmPassword: false,
	});
	const [error, setError] = useState<string | null>(null);
	const [success, setSuccess] = useState<string | null>(null);

	useEffect(() => {
		setUserInfo({
			id: user?.id || "",
			newFirstName: user?.firstName || "",
			newLastName: user?.lastName || "",
			newEmail: user?.email || "",
			newUsername: user?.userName || "",
			newPhoneNumber: user?.phoneNumber || null,
		});
		setPasswordInfo({
			id: user?.id || "",
			currentPassword: "",
			newPassword: "",
		});
		setConfirmPassword("");
		setError(null);
		setSuccess(null);
	}, [user]);

	const handleUpdateInfo = async (e: React.FormEvent) => {
		try {
			e.preventDefault();
			setError(null);
			setSuccess(null);
			await updateUser({
				...userNewInfo,
				id: user?.id || "",
			});
			setUserInfo({
				id: user?.id || "",
				newFirstName: "",
				newLastName: "",
				newEmail: "",
				newUsername: "",
				newPhoneNumber: "",
			});
			setSuccess("Profile updated successfully");
		} catch (err) {
			setError("Failed to update profile");
			console.error(err);
		}
	};

	const handleChangePassword = async (e: React.FormEvent) => {
		e.preventDefault();
		setError(null);
		setSuccess(null);
		if (passwordInfo.newPassword !== confirmPassword) {
			setError("Passwords do not match");
			return;
		}
		if (passwordInfo.newPassword === passwordInfo.currentPassword) {
			setError("New password cannot be the same as the current password");
			return;
		}
		try {
			const changePasswordRequest: ChangePasswordRequest = {
				id: user?.id || "",
				currentPassword: passwordInfo.currentPassword,
				newPassword: passwordInfo.newPassword,
			};
			await userApi.changePassword(changePasswordRequest);
			setPasswordInfo({
				id: user?.id || "",
				currentPassword: "",
				newPassword: "",
			});
			setConfirmPassword("");
			setSuccess("Password changed successfully");
		} catch (err) {
			setError("Failed to change password");
			console.error(err);
		}
	};

	const handleDeleteAccount = async () => {
		if (
			!window.confirm(
				"Are you sure you want to delete your account? This action cannot be undone."
			)
		) {
			return;
		}

		try {
			await userApi.deleteAccount(user?.id || "");
			logout();
		} catch (err) {
			setError("Failed to delete account");
			console.error(err);
		}
	};

	return (
		<div className="container mt-5">
			<h2 className="mb-4 text-light text-center">Account Settings</h2>
			{error && <div className="alert alert-danger">{error}</div>}
			{success && <div className="alert alert-success">{success}</div>}

			<div className="row">
				<div className="card mb-4">
					<div className="card-body">
						<h3 className="card-title mb-4">Profile Information</h3>
						<form onSubmit={handleUpdateInfo}>
							<div className="mb-3">
								<label className="form-label">First Name</label>
								<input
									type="text"
									className="form-control"
									value={userNewInfo.newFirstName || ""}
									onChange={(e) =>
										setUserInfo({
											...userNewInfo,
											newFirstName: e.target.value,
										})
									}
									placeholder={user?.firstName}
								/>
							</div>
							<div className="mb-3">
								<label className="form-label">Last Name</label>
								<input
									type="text"
									className="form-control"
									value={userNewInfo.newLastName || ""}
									onChange={(e) =>
										setUserInfo({
											...userNewInfo,
											newLastName: e.target.value,
										})
									}
									placeholder={user?.lastName}
								/>
							</div>
							<div className="mb-3">
								<label className="form-label">Email</label>
								<input
									type="email"
									className="form-control"
									value={userNewInfo.newEmail || ""}
									onChange={(e) =>
										setUserInfo({
											...userNewInfo,
											newEmail: e.target.value,
										})
									}
									placeholder={user?.email}
								/>
							</div>
							<div className="mb-3">
								<label className="form-label">Username</label>
								<input
									type="text"
									className="form-control"
									value={userNewInfo.newUsername || ""}
									onChange={(e) =>
										setUserInfo({
											...userNewInfo,
											newUsername: e.target.value,
										})
									}
									placeholder={user?.userName}
								/>
							</div>
							<button type="submit" className="btn btn-primary">
								Update Profile
							</button>
						</form>
					</div>
				</div>

				<div className="card mb-4">
					<div className="card-body">
						<h3 className="card-title mb-4">Change Password</h3>
						<form onSubmit={handleChangePassword}>
							<div className="mb-3">
								<label className="form-label">
									Current Password
								</label>
								<div className="input-group">
									<input
										type={
											passwordVisibility.currentPassword
												? "text"
												: "password"
										}
										className="form-control"
										value={passwordInfo.currentPassword}
										onChange={(e) =>
											setPasswordInfo({
												...passwordInfo,
												currentPassword: e.target.value,
											})
										}
									/>
									<button
										className="btn btn-outline-secondary"
										type="button"
										onClick={() =>
											setPasswordVisibility({
												...passwordVisibility,
												currentPassword:
													!passwordVisibility.currentPassword,
											})
										}
									>
										<i
											className={`bi bi-eye${
												passwordVisibility.currentPassword
													? "-slash"
													: ""
											}`}
										></i>
									</button>
								</div>
							</div>

							<div className="mb-3">
								<label className="form-label">
									New Password
								</label>
								<div className="input-group">
									<input
										type={
											passwordVisibility.newPassword
												? "text"
												: "password"
										}
										className="form-control"
										value={passwordInfo.newPassword}
										onChange={(e) =>
											setPasswordInfo({
												...passwordInfo,
												newPassword: e.target.value,
											})
										}
									/>
									<button
										className="btn btn-outline-secondary"
										type="button"
										onClick={() =>
											setPasswordVisibility({
												...passwordVisibility,
												newPassword:
													!passwordVisibility.newPassword,
											})
										}
									>
										<i
											className={`bi bi-eye${
												passwordVisibility.newPassword
													? "-slash"
													: ""
											}`}
										></i>
									</button>
								</div>
							</div>

							<div className="mb-3">
								<label className="form-label">
									Confirm New Password
								</label>
								<div className="input-group">
									<input
										type={
											passwordVisibility.confirmPassword
												? "text"
												: "password"
										}
										className="form-control"
										value={confirmPassword}
										onChange={(e) =>
											setConfirmPassword(e.target.value)
										}
									/>
									<button
										className="btn btn-outline-secondary"
										type="button"
										onClick={() =>
											setPasswordVisibility({
												...passwordVisibility,
												confirmPassword:
													!passwordVisibility.confirmPassword,
											})
										}
									>
										<i
											className={`bi bi-eye${
												passwordVisibility.confirmPassword
													? "-slash"
													: ""
											}`}
										></i>
									</button>
								</div>
							</div>
							<button type="submit" className="btn btn-primary">
								Change Password
							</button>
						</form>
					</div>
				</div>

				<div className="card bg-light">
					<div className="card-body">
						<h3 className="card-title text-danger mb-4">
							Danger Zone
						</h3>
						<button
							className="btn btn-danger"
							onClick={handleDeleteAccount}
						>
							Delete Account
						</button>
					</div>
				</div>
			</div>
		</div>
	);
}
