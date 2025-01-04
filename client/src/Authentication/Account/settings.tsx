import { useState } from "react";
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
		newUsername: user?.username || "",
		newPhoneNumber: user?.phoneNumber || "",
	});
	const [passwordInfo, setPasswordInfo] = useState<ChangePasswordRequest>({
		id: user?.id || "",
		currentPassword: "",
		newPassword: "",
	});
	const [error, setError] = useState<string | null>(null);
	const [success, setSuccess] = useState<string | null>(null);

	const handleUpdateInfo = async (e: React.FormEvent) => {
		e.preventDefault();
		setError(null);
		setSuccess(null);

		try {
			await userApi.updateUserInfo(userNewInfo);
			updateUser({
				id: user?.id || "",
				firstName: userNewInfo.newFirstName ?? user?.firstName ?? "",
				lastName: userNewInfo.newLastName ?? user?.lastName ?? "",
				email: userNewInfo.newEmail ?? user?.email ?? "",
				username: userNewInfo.newUsername ?? user?.username ?? "",
				phoneNumber:
					userNewInfo.newPhoneNumber ?? user?.phoneNumber ?? "",
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

		if (passwordInfo.newPassword !== passwordInfo.currentPassword) {
			setError("Passwords do not match");
			return;
		}

		try {
			const changePasswordRequest: ChangePasswordRequest = {
				id: user?.id || "",
				currentPassword: passwordInfo.currentPassword,
				newPassword: passwordInfo.newPassword,
			};
			await userApi.changePassword(changePasswordRequest);
			setSuccess("Password changed successfully");
			setPasswordInfo({
				id: user?.id || "",
				currentPassword: "",
				newPassword: "",
			});
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
				<div className="col-md-6 mb-4">
					<div className="card">
						<div className="card-body">
							<h3 className="card-title mb-4">
								Profile Information
							</h3>
							<form onSubmit={handleUpdateInfo}>
								<div className="mb-3">
									<label className="form-label">
										First Name
									</label>
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
									<label className="form-label">
										Last Name
									</label>
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
									<label className="form-label">
										Username
									</label>
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
										placeholder={user?.username}
									/>
								</div>
								<button
									type="submit"
									className="btn btn-primary"
								>
									Update Profile
								</button>
							</form>
						</div>
					</div>
				</div>

				<div className="col-md-6">
					<div className="card mb-4">
						<div className="card-body">
							<h3 className="card-title mb-4">Change Password</h3>
							<form onSubmit={handleChangePassword}>
								<div className="mb-3">
									<label className="form-label">
										Current Password
									</label>
									<input
										type="password"
										className="form-control"
										value={passwordInfo.currentPassword}
										onChange={(e) =>
											setPasswordInfo({
												...passwordInfo,
												currentPassword: e.target.value,
											})
										}
									/>
								</div>
								<div className="mb-3">
									<label className="form-label">
										New Password
									</label>
									<input
										type="password"
										className="form-control"
										value={passwordInfo.newPassword}
										onChange={(e) =>
											setPasswordInfo({
												...passwordInfo,
												newPassword: e.target.value,
											})
										}
									/>
								</div>
								<div className="mb-3">
									<label className="form-label">
										Confirm New Password
									</label>
									<input
										type="password"
										className="form-control"
										value={passwordInfo.newPassword}
										onChange={(e) =>
											setPasswordInfo({
												...passwordInfo,
												newPassword: e.target.value,
											})
										}
									/>
								</div>
								<button
									type="submit"
									className="btn btn-primary"
								>
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
		</div>
	);
}
