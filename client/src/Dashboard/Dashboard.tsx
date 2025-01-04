import { UseLists } from "./context/ListsContext";
import { TaskProvider } from "./context/TaskContext";
import MainContent from "./main/MainContent";
import Sidebar from "./sidebar/Sidebar";

export default function Dashboard() {
	const { isLoading, isSidebarOpen, toggleSidebar } = UseLists();

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
					<Sidebar />
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

				<TaskProvider>
					<MainContent />
				</TaskProvider>
			</div>
		</div>
	);
}
