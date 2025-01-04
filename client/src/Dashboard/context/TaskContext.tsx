import { createContext, useContext, useState, useMemo } from "react";
import { Task, TaskPriority } from "../../API/interfaces";
import { tasksApi } from "../../API/Calls/TasksCalls";
import { UseLists } from "./ListsContext";

interface TaskContextType {
	selectedTask: Task | null;
	setSelectedTask: (task: Task | null) => void;
	newTaskName: string;
	setNewTaskName: (name: string) => void;
	addTask: (listId: string, name: string) => Promise<void>;
	updateTask: (task: Task) => Promise<void>;
	toggleTask: (task: Task) => Promise<void>;
	deleteTask: (taskId: string) => Promise<void>;
	error: string | null;
}

const TaskContext = createContext<TaskContextType | null>(null);

export function TaskProvider({ children }: { children: React.ReactNode }) {
	const [selectedTask, setSelectedTask] = useState<Task | null>(null);
	const [newTaskName, setNewTaskName] = useState("");
	const [error, setError] = useState<string | null>(null);
	const { selectedList, setSelectedList } = UseLists();

	const addTask = useMemo(
		() => async (listId: string, name: string) => {
			if (!name.trim()) return;
			try {
				const newTask = await tasksApi.createTask({
					name,
					description: "",
					priority: TaskPriority.Low,
					listId,
				});

				if (selectedList) {
					setSelectedList({
						...selectedList,
						tasks: [...(selectedList.tasks || []), newTask],
					});
				}
				setNewTaskName("");
			} catch (err) {
				setError("Failed to create task");
				console.error(err);
			}
		},
		[selectedList, setSelectedList]
	);

	// ... other memoized handlers
	const updateTask = useMemo(
		() => async (task: Task) => {
			try {
				setSelectedTask(task);
				const result = await tasksApi.updateTask(task);
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
		},
		[selectedList, setSelectedList]
	);

	const toggleTask = useMemo(
		() => async (task: Task) => {
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
		},
		[selectedList, setSelectedList]
	);

	const deleteTask = useMemo(
		() => async (taskId: string) => {
			try {
				await tasksApi.deleteTask(taskId);
				if (selectedList && selectedList.tasks) {
					setSelectedList({
						...selectedList,
						tasks: selectedList.tasks.filter(
							(t) => t.id !== taskId
						),
					});
				}
			} catch (err) {
				setError("Failed to delete task");
				console.error(err);
			}
		},
		[selectedList, setSelectedList]
	);

	const contextValue = useMemo(
		() => ({
			selectedTask,
			setSelectedTask,
			newTaskName,
			setNewTaskName,
			addTask,
			updateTask,
			toggleTask,
			deleteTask,
			error,
		}),
		[
			selectedTask,
			newTaskName,
			addTask,
			updateTask,
			toggleTask,
			deleteTask,
			error,
		]
	);

	return (
		<TaskContext.Provider value={contextValue}>
			{children}
		</TaskContext.Provider>
	);
}

export const UseTasks = () => {
	const context = useContext(TaskContext);
	if (!context) {
		throw new Error("useTasks must be used within a TaskProvider");
	}
	return context;
};
