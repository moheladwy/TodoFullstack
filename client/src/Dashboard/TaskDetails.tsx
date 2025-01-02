import { useState } from "react";
import { Task, TaskPriority } from "../API/interfaces";
import { GetPriorityBadgeColor } from "./MainContentHelpers";

interface TaskDetailsProps {
	task: Task | null;
	onUpdateTask: (updatedTask: Task) => void;
	onClose: () => void;
}

export default function TaskDetails({
	task,
	onUpdateTask,
	onClose,
}: TaskDetailsProps) {
	const [nameError, setNameError] = useState<string | null>(null);

	if (!task) return null;

	const handleNameChange = (value: string) => {
		if (value.trim().length === 0) {
			setNameError("Task name cannot be empty");
			return;
		}
		setNameError(null);
		onUpdateTask({ ...task, name: value });
	};

	return (
		<div className="task-details bg-dark text-light border-start h-100 p-4">
			<div className="d-flex justify-content-between align-items-center mb-4">
				<h4>Task Details</h4>
				<button
					className="btn btn-sm btn-outline-secondary"
					onClick={onClose}
				>
					<i className="bi bi-x-lg"></i>
				</button>
			</div>

			<div className="mb-3">
				<label className="form-label">Name</label>
				<input
					type="text"
					className="form-control"
					min={1}
					value={task.name}
					onChange={(e) => handleNameChange(e.target.value)}
				/>
				{nameError && (
					<div className="invalid-feedback">{nameError}</div>
				)}
			</div>

			<div className="mb-3">
				<label className="form-label">Description</label>
				<div className="input-group">
					<textarea
						className="form-control"
						placeholder="Task description..."
						rows={3}
						value={task.description || ""}
						onChange={(e) =>
							onUpdateTask({
								...task,
								description: e.target.value,
							})
						}
					/>
				</div>
				<button
					className="btn btn-secondary w-100 mt-1"
					type="button"
					onClick={() =>
						onUpdateTask({ ...task, description: undefined })
					}
					disabled={!task.description}
					title="Clear description"
				>
					Clear
				</button>
			</div>

			<div className="mb-3">
				<label className="form-label">Priority</label>
				<select
					className="form-select"
					value={task.priority}
					onChange={(e) =>
						onUpdateTask({
							...task,
							priority: Number(e.target.value),
						})
					}
				>
					{Object.entries(TaskPriority)
						.filter(([key]) => isNaN(Number(key)))
						.map(([key, value]) => (
							<option key={value} value={value}>
								{key}
							</option>
						))}
				</select>
			</div>

			<div className="mb-3">
				<div className="form-check form-switch">
					<input
						className="form-check-input"
						type="checkbox"
						checked={task.isCompleted}
						onChange={(e) =>
							onUpdateTask({
								...task,
								isCompleted: e.target.checked,
							})
						}
					/>
					<label className="form-check-label">Completed</label>
				</div>
			</div>

			<div className="task-status mt-4">
				<span
					className={`badge bg-${GetPriorityBadgeColor(
						task.priority
					)} me-2`}
				>
					{TaskPriority[task.priority]}
				</span>
				<span
					className={`badge ${
						task.isCompleted ? "bg-success" : "bg-secondary"
					}`}
				>
					{task.isCompleted ? "Completed" : "Active"}
				</span>
			</div>
		</div>
	);
}
