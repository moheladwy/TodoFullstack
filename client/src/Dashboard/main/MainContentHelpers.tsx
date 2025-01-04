import { Task, TaskPriority } from "../../API/interfaces";

enum SortBy {
	Name = "name",
	Priority = "priority",
	Completed = "completed",
}

enum GroupBy {
	None = "none",
	Priority = "priority",
	Completed = "completed",
}

const SortTasks = (tasks: Task[], sortBy: SortBy): Task[] => {
	return [...tasks].sort((a, b) => {
		switch (sortBy) {
			case SortBy.Name:
				return a.name.localeCompare(b.name);
			case SortBy.Priority:
				return a.priority - b.priority;
			case SortBy.Completed:
				return a.isCompleted === b.isCompleted
					? 0
					: a.isCompleted
					? 1
					: -1;
			default:
				return 0;
		}
	});
};

const GroupTasks = (
	tasks: Task[],
	groupBy: GroupBy,
	sortBy: SortBy
): Record<string, Task[]> => {
	if (groupBy === "none") return { All: SortTasks(tasks, sortBy) };

	return SortTasks(tasks, sortBy).reduce((groups, task) => {
		let key = "";
		if (groupBy === "priority") {
			key = TaskPriority[task.priority];
		} else if (groupBy === "completed") {
			key = task.isCompleted ? "Completed" : "Active";
		}

		if (!groups[key]) groups[key] = [];
		groups[key].push(task);
		return groups;
	}, {} as Record<string, Task[]>);
};

const GetPriorityBadgeColor = (priority: TaskPriority): string => {
	switch (priority) {
		case TaskPriority.Urgent:
			return "danger";
		case TaskPriority.High:
			return "warning";
		case TaskPriority.Medium:
			return "info";
		case TaskPriority.Low:
			return "secondary";
		default:
			return "secondary";
	}
};

export { SortBy, GroupBy, SortTasks, GroupTasks, GetPriorityBadgeColor };
