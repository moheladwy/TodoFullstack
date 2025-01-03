import api from "../axios";
import {
	User,
	ChangePasswordRequest,
	UpdateUserInfoRequest,
} from "../interfaces";
import { USER_URLs } from "../URLs";

export const userApi = {
	getUser: async (userId: string): Promise<User> => {
		const response = await api.get<User>(`${USER_URLs.GET_USER}/${userId}`);
		return response.data;
	},

	changePassword: async (
		changePasswordRequest: ChangePasswordRequest
	): Promise<void> => {
		const response = await api.put(
			USER_URLs.CHANGE_PASSWORD,
			changePasswordRequest
		);
		return response.data;
	},

	updateUserInfo: async (
		updatedUserInfo: UpdateUserInfoRequest
	): Promise<void> => {
		const response = await api.put(
			USER_URLs.UPDATE_USER_INFO,
			updatedUserInfo
		);
		return response.data;
	},

	deleteAccount: async (userId: string): Promise<void> => {
		const response = await api.delete(
			`${USER_URLs.DELETE_ACCOUNT_BY_USER_ID}/${userId}`
		);
		return response.data;
	},
};
