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
	const [name, setName] = useState<string>(task?.name || "");
	const [nameError, setNameError] = useState<string | null>(null);
	const [description, setDescription] = useState<string>(
		task?.description || ""
	);
	const [descriptionError, setDescriptionError] = useState<string | null>(
		null
	);

	if (!task) return null;

	const handleNameChange = (value: string) => {
		if (value.trim().length === 0) {
			setNameError("Task name cannot be empty");
			return;
		}
		setNameError(null);
		setName(value);
	};

	const handleNameSave = () => {
		if (task.name.trim().length === 0) {
			setNameError("Task name cannot be empty");
			return;
		}
		setNameError(null);
		onUpdateTask({ ...task, name: name });
	};

	const handleDescriptionChange = (value: string) => {
		if (value.trim().length > 500) {
			setDescriptionError(
				"Task description cannot be more than 500 characters"
			);
			return;
		}
		setDescriptionError(null);
		setDescription(value);
	};

	const handleDescriptionClear = () => {
		setDescription("");
		if (task.description?.trim().length !== 0)
			onUpdateTask({ ...task, description: undefined });
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
					value={task.name}
					onChange={(e) => handleNameChange(e.target.value)}
				/>
				{nameError && (
					<div className="invalid-feedback">{nameError}</div>
				)}
				<button
					className="btn btn-primary mt-2 w-100"
					title="Save task name"
					onClick={handleNameSave}
				>
					Save
				</button>
			</div>

			<div className="mb-3">
				<label className="form-label">Description</label>
				<div className="input-group">
					<textarea
						className="form-control"
						placeholder="Task description..."
						rows={3}
						value={description}
						onChange={(e) =>
							handleDescriptionChange(e.target.value)
						}
					/>
				</div>
				{descriptionError && (
					<div className="invalid-feedback">{descriptionError}</div>
				)}
				<div className="description-actions d-flex justify-content-stretch mt-1">
					<button
						className="btn btn-danger flex-grow-1 me-1"
						type="button"
						onClick={handleDescriptionClear}
						disabled={
							!description || description.trim().length === 0
						}
						title="Clear description"
					>
						Clear
					</button>
					<button
						className="btn btn-primary flex-grow-1 ms-1"
						type="button"
						onClick={() =>
							onUpdateTask({ ...task, description: description })
						}
						disabled={
							!description || description === task.description
						}
						title="Save description"
					>
						Save
					</button>
				</div>
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
