export enum HttpMethods {
	GET = "GET",
	POST = "POST",
	PUT = "PUT",
	DELETE = "DELETE",
	PATCH = "PATCH",
}

export interface RegisterRequest {
	email: string;
	username: string;
	firstName: string;
	lastName: string;
	password: string;
}

export interface LoginRequest {
	email: string;
	password: string;
}

export interface AuthResponse {
	userId: string;
	accessToken: string;
	accessTokenExpirationDate: string;
	refreshToken: string;
	refreshTokenExpirationDate: string;
}

export interface User {
	id: string;
	email: string;
	userName: string;
	firstName: string;
	lastName: string;
	phoneNumber: string | null;
}

export interface ChangePasswordRequest {
	id: string;
	currentPassword: string;
	newPassword: string;
}

export interface UpdateUserInfoRequest {
	id: string;
	newFirstName: string | null;
	newLastName: string | null;
	newUsername: string | null;
	newEmail: string | null;
	newPhoneNumber: string | null;
}

export enum TaskPriority {
	Urgent = 0,
	High = 1,
	Medium = 2,
	Low = 3,
}

export interface List {
	id: string;
	name: string;
	description?: string;
	tasks?: Task[];
}

export interface Task {
	id: string;
	name: string;
	description?: string;
	priority: TaskPriority;
	isCompleted: boolean;
	listId: string;
	dueDate?: string;
}
