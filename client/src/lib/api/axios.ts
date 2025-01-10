import axios from "axios";
import { BASE_URL, AUTH_URLs } from "./URLs";

const api = axios.create({
	baseURL: BASE_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

api.interceptors.request.use(
	(config) => {
		const token = localStorage.getItem("accessToken");
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	}
);

api.interceptors.response.use(
	(response) => {
		return response;
	},
	async (error) => {
		const originalRequest = error.config;

		if (error.response.status === 401 && !originalRequest._retry) {
			originalRequest._retry = true;

			// Try to refresh the token
			const refreshToken = localStorage.getItem("refreshToken");
			if (!refreshToken) {
				return Promise.reject(error);
			}

			try {
				const response = await axios.post(
					AUTH_URLs.REFRESH_ACCESS_TOKEN_BY_REFRESH_TOKEN,
					refreshToken
				);
				const { accessToken } = response.data;

				localStorage.setItem("accessToken", accessToken);
				originalRequest.headers.Authorization = `Bearer ${accessToken}`;

				return api(originalRequest);
			} catch (refreshError) {
				return Promise.reject(refreshError);
			}
		}
		return Promise.reject(error);
	}
);

export default api;
