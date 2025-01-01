import api from "./axios";
import { Task } from "./interfaces";

export const tasksApi = {
	getTasksByListId: async (listId: string) => {
		const response = await api.get<Task[]>(`/Tasks/all-tasks/${listId}`);
		return response.data;
	},

	createTask: async (task: Omit<Task, "id" | "isCompleted">) => {
		const response = await api.post<Task>("/Tasks/add-task", {
			name: task.name,
			description: task.description,
			priority: task.priority,
			listId: task.listId,
		});
		return response.data;
	},

	updateTask: async (task: Task) => {
		const response = await api.put<Task>("/Tasks/update-task", {
			id: task.id,
			name: task.name,
			description: task.description,
			priority: task.priority,
			isCompleted: task.isCompleted,
			listId: task.listId,
		});
		return response.data;
	},

	deleteTask: async (id: string) => {
		await api.delete(`/Tasks/delete-task/${id}`);
	},
};
