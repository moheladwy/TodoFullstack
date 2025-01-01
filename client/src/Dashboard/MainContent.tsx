import { useState } from "react";
import { List, Task, TaskPriority } from "../API/interfaces";
import { tasksApi } from "../API/TasksActions";
import { UseAuth } from "../Authentication/context/AuthContext";

interface MainContentProps {
	selectedList: List | null;
	setSelectedList: (list: List) => void;
}

const sortTasks = (tasks: Task[]): Task[] => {
	return [...tasks].sort((a, b) => {
		if (a.isCompleted !== b.isCompleted) {
			return a.isCompleted ? 1 : -1;
		}
		return a.name.localeCompare(b.name);
	});
};

export default function MainContent({
	selectedList,
	setSelectedList,
}: MainContentProps) {
	const [newTaskName, setNewTaskName] = useState<string>("");
	const [error, setError] = useState<string | null>(null);
	const { isSidebarOpen } = UseAuth();

	const handleAddTask = async (e: React.FormEvent) => {
		e.preventDefault();
		if (!newTaskName.trim() || !selectedList) return;

		try {
			const newTask = await tasksApi.createTask({
				name: newTaskName, // Changed from Name
				description: "",
				priority: TaskPriority.Low,
				listId: selectedList.id,
			});

			setSelectedList({
				...selectedList,
				tasks: [...(selectedList.tasks || []), newTask],
			});
			setNewTaskName("");
		} catch (err) {
			setError("Failed to create task");
			console.error(err);
		}
	};

	const handleToggleTask = async (task: Task) => {
		try {
			const updatedTask = await tasksApi.updateTask({
				...task,
				isCompleted: !task.isCompleted,
			});

			if (selectedList && selectedList.tasks) {
				setSelectedList({
					...selectedList,
					tasks: selectedList.tasks.map((t) =>
						t.id === updatedTask.id ? updatedTask : t
					),
				});
			}
		} catch (err) {
			setError("Failed to update task");
			console.error(err);
		}
	};

	const handleDeleteTask = async (taskId: string) => {
		if (!selectedList) return;
		try {
			await tasksApi.deleteTask(taskId);
			setSelectedList({
				...selectedList,
				tasks: selectedList.tasks?.filter((task) => task.id !== taskId),
			});
		} catch (err) {
			setError("Failed to delete task");
			console.error(err);
		}
	};

	return (
		<main
			className={`${
				isSidebarOpen ? `col-md-9 ms-sm-auto col-lg-10` : `w-97`
			} px-md-4 t-smooth bg-light text-dark`}
			style={{
				borderTopLeftRadius: "1rem",
				height: "94vh",
				overflowY: "auto",
			}}
		>
			{selectedList ? (
				<>
					<div className="d-flex justify-content-stretch flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 px-0 mx-0 mb-3">
						{error && (
							<div className="alert alert-danger">{error}</div>
						)}
						<h1 className="h2 w-15 mx-3">{selectedList.name}</h1>
						<form
							onSubmit={handleAddTask}
							className="row g-1 w-100"
						>
							<div className="col">
								<input
									type="text"
									className="form-control"
									value={newTaskName}
									onChange={(e) =>
										setNewTaskName(e.target.value)
									}
									placeholder="Add a new task..."
								/>
							</div>
							<div className="col-auto">
								<button
									type="submit"
									className="btn btn-primary"
								>
									Add Task
								</button>
							</div>
						</form>
					</div>

					<div className="list-group mt-3">
						{selectedList?.tasks &&
							sortTasks(selectedList.tasks).map((task) => (
								<div
									key={task.id}
									className="list-group-item d-flex justify-content-between align-items-center"
								>
									<div className="form-check">
										<input
											className="form-check-input"
											type="checkbox"
											checked={task.isCompleted}
											onChange={() =>
												handleToggleTask(task)
											}
										/>
										<label
											className={`form-check-label ${
												task.isCompleted
													? "text-decoration-line-through"
													: ""
											}`}
										>
											{task.name}
										</label>
									</div>
									<button
										onClick={() =>
											handleDeleteTask(task.id)
										}
										className="btn btn-outline-danger btn-sm"
									>
										<i className="bi bi-trash-fill"></i>
									</button>
								</div>
							))}
					</div>
				</>
			) : (
				<div className="text-center mt-5 text-muted">
					Select a list or create a new one
				</div>
			)}
		</main>
	);
}
