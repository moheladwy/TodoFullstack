// import BASE_URL from .env file
const BASE_URL: string = import.meta.env.VITE_SERVER_URL;

// Auth URLs
const AUTH_URLs = {
	REGISTER: "/Auth/register",
	LOGIN: "/Auth/login",
	REFRESH_ACCESS_TOKEN_BY_REFRESH_TOKEN: "/Auth/refresh",
	LOGOUT: "/Auth/logout",
};

// Lists URLs
const LISTS_URLs = {
	GET_ALL_LISTS: "/Lists/all-lists",
	GET_LIST_BY_ID: "/Lists/get-list",
	CREATE_LIST: "/Lists/add-list",
	UPDATE_LIST: "/Lists/update-list",
	DELETE_LIST_BY_LIST_ID: "/Lists/delete-list",
};

// Tasks URLs
const TASKS_URLs = {
	GET_TASKS_BY_LIST_ID: "/Tasks/all-tasks",
	CREATE_TASK: "/Tasks/add-task",
	UPDATE_TASK: "/Tasks/update-task",
	DELETE_TASK_BY_TASK_ID: "/Tasks/delete-task",
};

// User URLs
const USER_URLs = {
	GET_USER: "/Account/get-user",
	CHANGE_PASSWORD: "/Account/change-password",
	UPDATE_USER_INFO: "/Account/update-user-info",
	DELETE_ACCOUNT_BY_USER_ID: "/Account/delete-account",
};

export { BASE_URL, AUTH_URLs, LISTS_URLs, TASKS_URLs, USER_URLs };
