import { Task, TaskPriority } from "../../API/interfaces";
import { UseLists } from "../context/ListsContext";
import { UseTasks } from "../context/TaskContext";
import {
	GroupTasks,
	GroupBy,
	GetPriorityBadgeColor,
	SortBy,
} from "./MainContentHelpers";

interface TasksProps {
	groupBy: GroupBy;
	sortBy: SortBy;
}

export default function Tasks({ groupBy, sortBy }: TasksProps) {
	const { selectedList } = UseLists();
	const { selectedTask, setSelectedTask, toggleTask, deleteTask } =
		UseTasks();

	const handleTaskSelect = (task: Task) => {
		if (task) {
			if (selectedTask && selectedTask.id === task.id) {
				setSelectedTask(null);
				return;
			}
			setSelectedTask(task);
		}
	};

	return (
		<div className="list-group mt-3">
			{selectedList?.tasks &&
				Object.entries(
					GroupTasks(selectedList.tasks, groupBy, sortBy)
				).map(([group, tasks]) => (
					<div key={group} className="mb-4 list-group">
						{groupBy !== GroupBy.None && (
							<h4 className="mb-3">{group}</h4>
						)}
						{tasks.map((task) => (
							<div
								key={task.id}
								className={`list-group-item py-3 d-flex justify-content-between align-items-center cursor-pointer ${
									selectedTask?.id === task.id
										? "selected"
										: ""
								}`}
							>
								<div className="form-check w-100">
									<input
										className="form-check-input"
										type="checkbox"
										checked={task.isCompleted}
										onChange={() => toggleTask(task)}
									/>
									<label
										className={`form-check-label w-100 cursor-pointer ${
											task.isCompleted
												? "text-decoration-line-through"
												: ""
										}`}
										onClick={() => handleTaskSelect(task)}
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
											<small
												className={`ms-2 ${
													selectedTask?.id === task.id
														? "text-light"
														: "text-muted"
												}`}
											>
												{task.description}
											</small>
										)}
									</label>
								</div>
								<button
									onClick={() => deleteTask(task.id)}
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
