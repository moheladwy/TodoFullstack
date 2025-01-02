export interface AuthResponse {
	userId: string;
	accessToken: string;
	accessTokenExpirationDate: string;
	refreshToken: string;
	refreshTokenExpirationDate: string;
}

export enum HttpMethods {
	GET = "GET",
	POST = "POST",
	PUT = "PUT",
	DELETE = "DELETE",
	PATCH = "PATCH",
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
}
