import api from "../axios";
import { RegisterRequest, LoginRequest, AuthResponse } from "../interfaces";
import { AUTH_URLs } from "../URLs";

const SUCCESSFUL_RESPONSE = 200;

export const authApi = {
	register: async (userInfo: RegisterRequest): Promise<AuthResponse> => {
		const response = await api.post(AUTH_URLs.REGISTER, userInfo);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Registration Faild");
		return response.data;
	},

	login: async (loginRequest: LoginRequest): Promise<AuthResponse> => {
		const response = await api.post(AUTH_URLs.LOGIN, loginRequest);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Login Faild");
		return response.data;
	},

	refresh: async (refreshToken: string): Promise<AuthResponse> => {
		const response = await api.post(
			AUTH_URLs.REFRESH_ACCESS_TOKEN_BY_REFRESH_TOKEN,
			refreshToken
		);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Refresh Access Token Faild");
		return response.data;
	},

	logout: async () => {
		const response = await api.post(AUTH_URLs.LOGOUT);
		if (response.status !== SUCCESSFUL_RESPONSE)
			throw new Error("Logout Faild");
		return response.data;
	},
};
