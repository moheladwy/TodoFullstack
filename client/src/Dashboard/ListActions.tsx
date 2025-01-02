import { List } from "../API/interfaces";
import { GroupBy, SortBy } from "./MainContentHelpers";

interface ListActionsProps {
	selectedList: List;
	sortBy: SortBy;
	groupBy: GroupBy;
	setSortBy: (sortBy: SortBy) => void;
	setGroupBy: (groupBy: GroupBy) => void;
	error: string | null;
	newTaskName: string;
	setNewTaskName: (newTaskName: string) => void;
	handleAddTask: (e: React.FormEvent) => void;
}

export default function ListActions({
	selectedList,
	sortBy,
	groupBy,
	setSortBy,
	setGroupBy,
	error,
	newTaskName,
	setNewTaskName,
	handleAddTask,
}: ListActionsProps) {
	return (
		<div className="d-flex flex-column justify-content-center flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 px-0 mx-0 mb-3">
			{error && <div className="alert alert-danger">{error}</div>}
			<div className="list-actions mb-2 d-flex justify-content-between align-items-center w-100">
				<h1 className="h2 w-15 mx-3">{selectedList.name}</h1>
				<div className="d-flex gap-2">
					<div className="btn-group">
						<button
							type="button"
							className="btn btn-secondary dropdown-toggle"
							data-bs-toggle="dropdown"
						>
							Sort by
						</button>
						<ul className="dropdown-menu">
							<li>
								<button
									className={`dropdown-item ${
										sortBy === "name" ? "active" : ""
									}`}
									onClick={() => setSortBy(SortBy.Name)}
								>
									Name
								</button>
							</li>
							<li>
								<button
									className={`dropdown-item ${
										sortBy === "priority" ? "active" : ""
									}`}
									onClick={() => setSortBy(SortBy.Priority)}
								>
									Priority
								</button>
							</li>
							<li>
								<button
									className={`dropdown-item ${
										sortBy === "completed" ? "active" : ""
									}`}
									onClick={() => setSortBy(SortBy.Completed)}
								>
									Completed
								</button>
							</li>
						</ul>
					</div>

					<div className="btn-group">
						<button
							type="button"
							className="btn btn-secondary dropdown-toggle"
							data-bs-toggle="dropdown"
						>
							Group by
						</button>
						<ul className="dropdown-menu">
							<li>
								<button
									className={`dropdown-item ${
										groupBy === "none" ? "active" : ""
									}`}
									onClick={() => setGroupBy(GroupBy.None)}
								>
									None
								</button>
							</li>
							<li>
								<button
									className={`dropdown-item ${
										groupBy === "priority" ? "active" : ""
									}`}
									onClick={() => setGroupBy(GroupBy.Priority)}
								>
									Priority
								</button>
							</li>
							<li>
								<button
									className={`dropdown-item ${
										groupBy === "completed" ? "active" : ""
									}`}
									onClick={() =>
										setGroupBy(GroupBy.Completed)
									}
								>
									Status
								</button>
							</li>
						</ul>
					</div>
				</div>
			</div>
			<form onSubmit={handleAddTask} className="row g-1 w-100">
				<div className="col">
					<input
						type="text"
						className="form-control"
						value={newTaskName}
						onChange={(e) => setNewTaskName(e.target.value)}
						placeholder="Add a new task..."
					/>
				</div>
				<div className="col-auto">
					<button type="submit" className="btn btn-primary">
						Add Task
					</button>
				</div>
			</form>
		</div>
	);
}
