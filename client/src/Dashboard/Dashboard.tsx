import { useState, useEffect } from "react";
import { List } from "../API/interfaces";
import { tasksApi } from "../API/Calls/TasksCalls";
import { listsApi } from "../API/Calls/ListsCalls";
import { UseAuth } from "../Authentication/context/AuthContext";
import MainContent from "./MainContent";
import Sidebar from "./Sidebar";

export default function Dashboard() {
	const [lists, setLists] = useState<List[]>([]);
	const [selectedList, setSelectedList] = useState<List | null>(null);
	const [isLoading, setIsLoading] = useState(true);
	const { isSidebarOpen, toggleSidebar } = UseAuth();

	useEffect(() => {
		const fetchLists = async () => {
			try {
				setIsLoading(true);
				const response = await listsApi.getAllLists();
				const sortedLists = [...response].sort((a, b) => {
					const nameA = a.name.toLowerCase();
					const nameB = b.name.toLowerCase();
					const numA = parseInt(nameA) || 0;
					const numB = parseInt(nameB) || 0;
					if (numA && numB) {
						return numA - numB;
					}
					return nameA.localeCompare(nameB);
				});
				setLists(sortedLists);
				if (sortedLists.length > 0) {
					setSelectedList(sortedLists[0]);
				}
			} catch (err: Error | unknown) {
				console.error("Fetch error:", err);
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
					setSelectedList({
						...selectedList,
						tasks,
					});
				} catch (err) {
					console.error("Failed to fetch tasks:", err);
				}
			}
		};
		fetchTasks();
	}, [selectedList?.id]);

	const handleListUpdate = (updatedList: List) => {
		setLists(
			lists.map((list) =>
				list.id === updatedList.id ? updatedList : list
			)
		);
		setSelectedList(updatedList);
	};

	if (isLoading)
		return (
			<div className="text-center text-light fs-3 py-5 justtify-content-center">
				Loading...
			</div>
		);

	return (
		<div className="container-fluid h-90 bg-dark text-light">
			<div className="row bg-dark">
				{isSidebarOpen ? (
					<Sidebar
						lists={lists}
						setLists={setLists}
						selectedList={selectedList}
						setSelectedList={setSelectedList}
					/>
				) : (
					<div
						className="d-flex flex-column bg-dark text-light t-smooth"
						style={{
							height: "100vh",
							width: "3%",
							borderRadius: "50%",
						}}
					>
						<i
							className="bi bi-layout-sidebar-inset-reverse fs-4 cursor-pointer"
							onClick={toggleSidebar}
						></i>
					</div>
				)}

				<MainContent
					selectedList={selectedList}
					setSelectedList={setSelectedList}
					onListUpdate={handleListUpdate}
				/>
			</div>
		</div>
	);
}
