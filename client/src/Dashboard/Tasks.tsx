import { List, Task, TaskPriority } from "../API/interfaces";
import {
	GroupTasks,
	GroupBy,
	GetPriorityBadgeColor,
	SortBy,
} from "./MainContentHelpers";

interface TasksProps {
	selectedList: List;
	handleDeleteTask: (taskId: string) => void;
	handleToggleTask: (task: Task) => void;
	handleTaskSelect: (task: Task) => void;
	selectedTask: Task | null;
	groupBy: GroupBy;
	sortBy: SortBy;
}

export default function Tasks({
	selectedList,
	handleDeleteTask,
	handleToggleTask,
	handleTaskSelect,
	selectedTask,
	groupBy,
	sortBy,
}: TasksProps) {
	return (
		<div className="list-group mt-3">
			{selectedList?.tasks &&
				Object.entries(
					GroupTasks(selectedList.tasks, groupBy, sortBy)
				).map(([group, tasks]) => (
					<div key={group} className="mb-4">
						{groupBy !== GroupBy.None && (
							<h4 className="mb-3">{group}</h4>
						)}
						{tasks.map((task) => (
							<div
								key={task.id}
								className={`list-group-item d-flex justify-content-between align-items-center cursor-pointer ${
									selectedTask?.id === task.id ? "active" : ""
								}`}
								onClick={() => handleTaskSelect(task)}
							>
								<div className="form-check">
									<input
										className="form-check-input"
										type="checkbox"
										checked={task.isCompleted}
										onChange={() => handleToggleTask(task)}
									/>
									<label
										className={`form-check-label ${
											task.isCompleted
												? "text-decoration-line-through"
												: ""
										}`}
									>
										<span
											className={`badge bg-${GetPriorityBadgeColor(
												task.priority
											)} ms-2 mx-3`}
										>
											{TaskPriority[task.priority]}
										</span>
										{task.name}
										{task.description && (
											<small className="text-muted ms-2">
												{task.description}
											</small>
										)}
									</label>
								</div>
								<button
									onClick={() => handleDeleteTask(task.id)}
									className="btn btn-outline-danger btn-sm"
								>
									<i className="bi bi-trash-fill"></i>
								</button>
							</div>
						))}
					</div>
				))}
		</div>
	);
}
