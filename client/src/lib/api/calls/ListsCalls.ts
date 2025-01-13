import api from "../axios";
import { CreateListRequest, List, UpdateListRequest } from "../interfaces";
import { LISTS_URLs } from "../URLs";

const SUCCESSFUL_STATUS = 200;

export const listsApi = {
	getAllLists: async (): Promise<List[]> => {
		const response = await api.get<List[]>(LISTS_URLs.GET_ALL_LISTS);
		if (response.status !== SUCCESSFUL_STATUS) {
			throw new Error("Failed to fetch lists");
		}
		return response.data;
	},

	getListById: async (id: string): Promise<List> => {
		const response = await api.get<List>(
			`${LISTS_URLs.GET_LIST_BY_ID}/${id}`
		);
		if (response.status !== SUCCESSFUL_STATUS) {
			throw new Error("Failed to fetch list by id: " + id);
		}
		return response.data;
	},

	createList: async (createListRequest: CreateListRequest): Promise<List> => {
		const response = await api.post<List>(LISTS_URLs.CREATE_LIST, {
			name: createListRequest.name,
			description: createListRequest.description,
			userId: createListRequest.userId,
		});
		if (response.status !== SUCCESSFUL_STATUS) {
			throw new Error("Failed to create list: " + name);
		}
		return response.data;
	},

	updateList: async (
		updateListRequest: UpdateListRequest
	): Promise<List> => {
		const response = await api.put<List>(LISTS_URLs.UPDATE_LIST, {
			id: updateListRequest.id,
			name: updateListRequest.name,
			description: updateListRequest.description,
		});
		if (response.status !== SUCCESSFUL_STATUS) {
			throw new Error("Failed to update list by id: " + updateListRequest.id);
		}
		return response.data;
	},

	deleteList: async (id: string): Promise<void> => {
		const response = await api.delete(
			`${LISTS_URLs.DELETE_LIST_BY_LIST_ID}/${id}`
		);
		if (response.status !== SUCCESSFUL_STATUS) {
			throw new Error("Failed to delete list by id: " + id);
		}
	},
};
