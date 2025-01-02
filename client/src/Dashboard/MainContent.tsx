import { useState } from "react";
import { List, Task, TaskPriority } from "../API/interfaces";
import { tasksApi } from "../API/TasksActions";
import { UseAuth } from "../Authentication/context/AuthContext";
import ListActions from "./ListActions";
import { GroupBy, SortBy } from "./MainContentHelpers";
import Tasks from "./Tasks";
import TaskDetails from "./TaskDetails";

interface MainContentProps {
	selectedList: List | null;
	setSelectedList: (list: List) => void;
}

export default function MainContent({
	selectedList,
	setSelectedList,
}: MainContentProps) {
	const [newTaskName, setNewTaskName] = useState<string>("");
	const [error, setError] = useState<string | null>(null);
	const [sortBy, setSortBy] = useState<SortBy>(SortBy.Name);
	const [groupBy, setGroupBy] = useState<GroupBy>(GroupBy.Completed);
	const [selectedTask, setSelectedTask] = useState<Task | null>(null);
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

	const handleTaskSelect = (task: Task) => {
		if (selectedTask && selectedTask.id === task.id) {
			setSelectedTask(null);
			return;
		}
		setSelectedTask(task);
	};

	const handleTaskUpdate = async (updatedTask: Task) => {
		try {
			setSelectedTask(updatedTask);
			const result = await tasksApi.updateTask(updatedTask);
			if (selectedList && selectedList.tasks) {
				setSelectedList({
					...selectedList,
					tasks: selectedList.tasks.map((t) =>
						t.id === result.id ? result : t
					),
				});
			}
		} catch (err) {
			setError("Failed to update task");
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
			} pl-md-4 t-smooth bg-white text-dark`}
			style={{
				height: "94vh",
				overflowY: "auto",
			}}
		>
			{selectedList ? (
				<div className="row h-100">
					<div
						className={`${selectedTask ? "col-md-9" : "col-12"}`}
						style={{
							borderTopRightRadius: `${
								selectedTask ? "2rem" : "0rem"
							}`,
						}}
					>
						<ListActions
							selectedList={selectedList}
							sortBy={sortBy}
							groupBy={groupBy}
							setSortBy={setSortBy}
							setGroupBy={setGroupBy}
							error={error}
							newTaskName={newTaskName}
							setNewTaskName={setNewTaskName}
							handleAddTask={handleAddTask}
						/>
						<Tasks
							selectedList={selectedList}
							handleDeleteTask={handleDeleteTask}
							handleToggleTask={handleToggleTask}
							handleTaskSelect={handleTaskSelect}
							selectedTask={selectedTask}
							groupBy={groupBy}
							sortBy={sortBy}
						/>
					</div>
					{selectedTask && (
						<div className="col-md-3 h-100 px-0">
							<TaskDetails
								task={selectedTask}
								onUpdateTask={handleTaskUpdate}
								onClose={() => setSelectedTask(null)}
							/>
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
