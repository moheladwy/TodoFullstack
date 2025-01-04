import { useState } from "react";
import { GroupBy, SortBy } from "./MainContentHelpers";
import { UseLists } from "../context/ListsContext";
import { UseTasks } from "../context/TaskContext";

interface ListActionsProps {
	sortBy: SortBy;
	groupBy: GroupBy;
	setSortBy: (sortBy: SortBy) => void;
	setGroupBy: (groupBy: GroupBy) => void;
}

export default function ListHeader({
	sortBy,
	groupBy,
	setSortBy,
	setGroupBy,
}: ListActionsProps) {
	const { selectedList, updateList } = UseLists();
	const { addTask } = UseTasks();

	const [isEditing, setIsEditing] = useState(false);
	const [editedName, setEditedName] = useState(selectedList?.name || "");
	const [editedDescription, setEditedDescription] = useState(
		selectedList?.description || ""
	);
	const [newTaskName, setNewTaskName] = useState("");
	const [error, setError] = useState<string | null>(null);

	if (!selectedList) return null;

	const handleUpdateListClick = () => {
		setIsEditing(true);
		setEditedName(selectedList.name || "");
		setEditedDescription(selectedList.description || "");
		setError(null);
	};

	const handleSubmit = (e: React.FormEvent) => {
		e.preventDefault();
		if (editedName.trim().length !== 0) {
			setError("List name cannot be empty");
		}
		updateList({
			name: editedName,
			description: editedDescription,
		});
		setIsEditing(false);
		setError(null);
	};

	return (
		<div className="d-flex flex-column justify-content-center flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 px-0 mx-0 mb-3">
			<div className="list-actions mb-4 d-flex justify-content-between align-items-center w-100">
				{isEditing ? (
					<form
						onSubmit={handleSubmit}
						className="d-flex flex-column mx-3 w-75"
					>
						<input
							type="text"
							className="form-control h2 mb-2"
							value={editedName}
							onChange={(e) => setEditedName(e.target.value)}
							autoFocus
							placeholder="List name..."
						/>
						<textarea
							className="form-control"
							value={editedDescription}
							onChange={(e) =>
								setEditedDescription(e.target.value)
							}
							placeholder="List description..."
							rows={2}
						/>
						<div className="mt-2">
							<button
								type="submit"
								className="btn btn-primary me-2"
							>
								Save
							</button>
							<button
								type="button"
								className="btn btn-secondary"
								onClick={() => {
									setIsEditing(false);
									setEditedName(selectedList?.name || "");
									setEditedDescription(
										selectedList?.description || ""
									);
								}}
							>
								Cancel
							</button>
						</div>
					</form>
				) : (
					<div className="mx-3 w-75" onClick={handleUpdateListClick}>
						<h1 className="h2 mb-1">{selectedList.name}</h1>
						{selectedList.description && (
							<p className="text-muted mb-0">
								{selectedList.description}
							</p>
						)}
					</div>
				)}
				<div className="d-flex gap-2">
					<button
						type="button"
						className="btn btn-outline-secondary mx-1"
						onClick={handleUpdateListClick}
						title="Edit list name"
					>
						<i className="bi bi-pencil"></i>
					</button>
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
			{error && <div className="alert alert-danger">{error}</div>}{" "}
			<form
				onSubmit={(e) => {
					e.preventDefault();
					addTask(selectedList.id, newTaskName);
				}}
				className="row g-1 w-100"
			>
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
