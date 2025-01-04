import { useEffect, useState } from "react";
import { TaskPriority } from "../../API/interfaces";
import { GetPriorityBadgeColor } from "./MainContentHelpers";
import { UseTasks } from "../context/TaskContext";

export default function TaskDetails() {
	const { selectedTask, setSelectedTask, updateTask } = UseTasks();
	const [name, setName] = useState<string>(selectedTask?.name || "");
	const [nameError, setNameError] = useState<string | null>(null);
	const [description, setDescription] = useState<string>(
		selectedTask?.description || ""
	);
	const [descriptionError, setDescriptionError] = useState<string | null>(
		null
	);

	if (!selectedTask) return null;

	// eslint-disable-next-line react-hooks/rules-of-hooks
	useEffect(() => {
		setName(selectedTask.name || "");
		setDescription(selectedTask.description || "");
		setNameError(null);
		setDescriptionError(null);
	}, [selectedTask]);

	const handleNameChange = (value: string) => {
		setNameError(null);
		setName(value);
	};

	const handleNameSave = () => {
		try {
			if (name.trim().length === 0) {
				setNameError("Task name cannot be empty");
				return;
			}
			setNameError(null);
			updateTask({ ...selectedTask, name: name });
		} catch (error) {
			console.error(error);
		}
	};

	const handleDescriptionChange = (value: string) => {
		try {
			if (value.trim().length > 500) {
				setDescriptionError(
					"Task description cannot be more than 500 characters"
				);
				return;
			}
			setDescriptionError(null);
			setDescription(value);
		} catch (error) {
			console.error(error);
		}
	};

	const handleDescriptionClear = () => {
		try {
			setDescription("");
			if (selectedTask.description?.trim().length !== 0)
				updateTask({ ...selectedTask, description: "" });
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="task-details bg-dark text-light border-start h-100 p-4">
			<div className="d-flex justify-content-between align-items-center mb-4">
				<h4>Task Details</h4>
				<button
					className="btn btn-sm btn-outline-secondary"
					onClick={() => setSelectedTask(null)}
				>
					<i className="bi bi-x-lg"></i>
				</button>
			</div>

			<div className="mb-3">
				<label className="form-label">Name</label>
				<input
					type="text"
					className="form-control"
					value={name}
					onChange={(e) => handleNameChange(e.target.value)}
				/>
				{nameError && (
					<div className="invalid-feedback">{nameError}</div>
				)}
				<button
					className="btn btn-primary mt-2 w-100"
					title="Save task name"
					onClick={handleNameSave}
					disabled={
						!name ||
						name.trim().length === 0 ||
						name === selectedTask.name
					}
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
							updateTask({
								...selectedTask,
								description: description,
							})
						}
						disabled={
							!description ||
							description === selectedTask.description
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
					value={selectedTask.priority}
					onChange={(e) =>
						selectedTask &&
						updateTask({
							...selectedTask,
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
						checked={selectedTask.isCompleted}
						onChange={(e) =>
							updateTask({
								...selectedTask,
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
						selectedTask.priority
					)} me-2`}
				>
					{TaskPriority[selectedTask.priority]}
				</span>
				<span
					className={`badge ${
						selectedTask.isCompleted ? "bg-success" : "bg-secondary"
					}`}
				>
					{selectedTask.isCompleted ? "Completed" : "Active"}
				</span>
			</div>
		</div>
	);
}
