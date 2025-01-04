import { createContext, useContext, useState, useEffect, useMemo } from "react";
import { List } from "../../API/interfaces";
import { listsApi } from "../../API/Calls/ListsCalls";
import { tasksApi } from "../../API/Calls/TasksCalls";

interface ListsContextType {
	lists: List[];
	selectedList: List | null;
	isLoading: boolean;
	isSidebarOpen: boolean;
	setLists: (lists: List[]) => void;
	setSelectedList: (list: List | null) => void;
	addList: (name: string) => Promise<void>;
	updateList: (request: UpdateListRequest) => Promise<void>;
	deleteList: (id: string) => Promise<void>;
	toggleSidebar: () => void;
}

interface UpdateListRequest {
	name: string | null;
	description: string | null;
}

const ListsContext = createContext<ListsContextType | null>(null);

export function ListsProvider({ children }: { children: React.ReactNode }) {
	const [lists, setLists] = useState<List[]>([]);
	const [selectedList, setSelectedList] = useState<List | null>(null);
	const [isLoading, setIsLoading] = useState(true);
	const [isSidebarOpen, setIsSidebarOpen] = useState(() => {
		const saved = localStorage.getItem("sidebarOpen");
		return saved !== null ? JSON.parse(saved) : true;
	});

	// Memoize sorted lists
	const sortedLists = useMemo(() => {
		return [...lists].sort((a, b) => {
			const nameA = a.name.toLowerCase();
			const nameB = b.name.toLowerCase();
			const numA = parseInt(nameA) || 0;
			const numB = parseInt(nameB) || 0;
			if (numA && numB) {
				return numA - numB;
			}
			return nameA.localeCompare(nameB);
		});
	}, [lists]);

	// Memoize handler functions
	const updateList = useMemo(
		() => async (updateListRequest: UpdateListRequest) => {
			try {
				if (!selectedList) return;
				const updatedList = await listsApi.updateList(
					selectedList.id,
					updateListRequest.name || selectedList.name,
					updateListRequest.description ?? selectedList.description
				);

				const updatedListWithTasks = {
					...updatedList,
					tasks: selectedList.tasks,
				};

				setLists((currentLists) =>
					currentLists.map((list) =>
						list.id === updatedList.id ? updatedListWithTasks : list
					)
				);
				setSelectedList(updatedListWithTasks);
			} catch (err) {
				console.error("Failed to update list:", err);
				throw err;
			}
		},
		[selectedList]
	);

	const addList = useMemo(
		() => async (name: string) => {
			try {
				const newList = await listsApi.createList(name);
				setLists((currentLists) => [...currentLists, newList]);
			} catch (err) {
				console.error("Failed to create list:", err);
				throw err;
			}
		},
		[]
	);

	const deleteList = useMemo(
		() => async (id: string) => {
			try {
				await listsApi.deleteList(id);
				setLists((currentLists) =>
					currentLists.filter((list) => list.id !== id)
				);
				setSelectedList((current) =>
					current?.id === id ? lists[0] || null : current
				);
			} catch (err) {
				console.error("Failed to delete list:", err);
				throw err;
			}
		},
		[lists]
	);

	useEffect(() => {
		const fetchLists = async () => {
			try {
				setIsLoading(true);
				const response = await listsApi.getAllLists();
				setLists(response);
				if (response.length > 0) {
					setSelectedList(response[0]);
				}
			} catch (err) {
				console.error("Failed to fetch lists:", err);
			} finally {
				setIsLoading(false);
			}
		};
		fetchLists();
	}, []);

	useEffect(() => {
		setSelectedList(selectedList);
	}, [selectedList]);

	useEffect(() => {
		const fetchTasks = async () => {
			if (selectedList) {
				try {
					const tasks = await tasksApi.getTasksByListId(
						selectedList.id
					);
					setSelectedList((current) =>
						current ? { ...current, tasks } : null
					);
				} catch (err) {
					console.error("Failed to fetch tasks:", err);
				}
			}
		};
		fetchTasks();
	}, [selectedList?.id]);

	const toggleSidebar = useMemo(
		() => () => {
			setIsSidebarOpen((isSidebarOpen: boolean) => {
				const newState = !isSidebarOpen;
				localStorage.setItem("sidebarOpen", JSON.stringify(newState));
				return newState;
			});
		},
		[]
	);

	// Memoize context value
	const contextValue = useMemo(
		() => ({
			lists: sortedLists,
			selectedList,
			isLoading,
			isSidebarOpen,
			setLists,
			setSelectedList,
			updateList,
			addList,
			deleteList,
			toggleSidebar,
		}),
		[
			sortedLists,
			selectedList,
			isLoading,
			isSidebarOpen,
			updateList,
			addList,
			deleteList,
			toggleSidebar,
		]
	);

	return (
		<ListsContext.Provider value={contextValue}>
			{children}
		</ListsContext.Provider>
	);
}

export const UseLists = () => {
	const context = useContext(ListsContext);
	if (!context) {
		throw new Error("UseLists must be used within a ListsProvider");
	}
	return context;
};
