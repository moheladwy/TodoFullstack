import api from "./axios";
import { List } from "./interfaces";

export const listsApi = {
	getAllLists: async () => {
		const response = await api.get<List[]>("/Lists/all-lists");
		return response.data;
	},

	getListById: async (id: string) => {
		const response = await api.get<List>(`/Lists/get-list/${id}`);
		return response.data;
	},

	createList: async (name: string, description?: string) => {
		const response = await api.post<List>("/Lists/add-list", {
			name,
			description,
		});
		return response.data;
	},

	updateList: async (id: string, name: string, description?: string) => {
		const response = await api.put<List>("/Lists/update-list", {
			id,
			name,
			description,
		});
		return response.data;
	},

	deleteList: async (id: string) => {
		await api.delete(`/Lists/delete-list/${id}`);
	},
};
