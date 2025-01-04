import { useState } from "react";
import { GroupBy, SortBy } from "./MainContentHelpers";
import { UseLists } from "../context/ListsContext";
import { UseTasks } from "../context/TaskContext";
import ListHeader from "./ListHeader";
import TaskDetails from "./TaskDetails";
import Tasks from "./Tasks";

export default function MainContent() {
	const { isSidebarOpen, selectedList } = UseLists();
	const { selectedTask } = UseTasks();
	const [sortBy, setSortBy] = useState<SortBy>(SortBy.Name);
	const [groupBy, setGroupBy] = useState<GroupBy>(GroupBy.Completed);

	return (
		<main
			className={`${
				isSidebarOpen ? `col-md-9 ms-sm-auto col-lg-10` : `w-97`
			} pl-md-4 t-smooth bg-white text-dark`}
			style={{
				height: "94vh",
				overflowY: "auto",
			}}
		>
			{selectedList ? (
				<div className="row h-100">
					<div className={`${selectedTask ? "col-md-9" : "col-12"}`}>
						<ListHeader
							sortBy={sortBy}
							groupBy={groupBy}
							setSortBy={setSortBy}
							setGroupBy={setGroupBy}
						/>
						<Tasks groupBy={groupBy} sortBy={sortBy} />
					</div>
					{selectedTask && (
						<div className="col-md-3 h-100 px-0">
							<TaskDetails />
						</div>
					)}
				</div>
			) : (
				<div className="text-center mt-5 text-muted">
					Select a list or create a new one
				</div>
			)}
		</main>
	);
}
