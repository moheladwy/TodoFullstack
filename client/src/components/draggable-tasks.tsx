import {
  DndContext,
  closestCenter,
  PointerSensor,
  useSensor,
  useSensors,
  DragEndEvent,
} from '@dnd-kit/core';
import {
  arrayMove,
  SortableContext,
  verticalListSortingStrategy,
  useSortable,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { useAppStore } from "@/lib/store/useStore";
import { Task, TaskOrder } from '@/lib/api/interfaces';
import { TaskItem } from '@/pages/Tasks/componenets/TaskItem';
import { useMemo } from 'react';
import { SortOption } from '@/pages/Tasks/componenets/TaskFilters';

interface DraggableTaskItemProps {
  task: Task;
}

function DraggableTaskItem({ task }: DraggableTaskItemProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: task.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      className="touch-none"
    >
      <div className="cursor-grab active:cursor-grabbing">
        <TaskItem task={task} />
      </div>
    </div>
  );
}

interface DraggableTasksProps {
  tasks: Task[];
  sortOption?: SortOption;
}

export function DraggableTasks({ tasks, sortOption = SortOption.CustomOrder }: DraggableTasksProps) {
  const { selectedListId, taskOrders, reorderTasks } = useAppStore();
  
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 5, // Minimum distance for drag activation
      },
    })
  );

  const sortedTasks = useMemo(() => {
    if (!selectedListId) return tasks;

    let orderedTasks = [...tasks];
    const currentOrders = taskOrders[selectedListId] || [];

    // First apply custom order from drag-and-drop if no sort option is active
    if (sortOption === SortOption.CustomOrder) {
      const tasksWithOrder = orderedTasks.map((task, index) => {
        const orderItem = currentOrders.find(order => order.taskId === task.id);
        return {
          ...task,
          order: orderItem?.order ?? index
        };
      });
      orderedTasks = tasksWithOrder.sort((a, b) => (a.order ?? 0) - (b.order ?? 0));
    } else {
      // Apply sorting based on the selected option
      switch (sortOption) {
        case SortOption.NameAsc:
          orderedTasks.sort((a, b) => a.name.localeCompare(b.name));
          break;
        case SortOption.NameDesc:
          orderedTasks.sort((a, b) => b.name.localeCompare(a.name));
          break;
        case SortOption.PriorityHighToLow:
          orderedTasks.sort((a, b) => a.priority - b.priority);
          break;
        case SortOption.PriorityLowToHigh:
          orderedTasks.sort((a, b) => b.priority - a.priority);
          break;
      }
    }

    return orderedTasks;
  }, [tasks, selectedListId, taskOrders, sortOption]);

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || active.id === over.id || !selectedListId) return;

    const oldIndex = sortedTasks.findIndex(task => task.id === active.id);
    const newIndex = sortedTasks.findIndex(task => task.id === over.id);

    const newSortedTasks = arrayMove(sortedTasks, oldIndex, newIndex);
    const newOrder: TaskOrder[] = newSortedTasks.map((task, index) => ({
      taskId: task.id,
      order: index,
    }));

    reorderTasks(selectedListId, newOrder);
  };

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragEnd={handleDragEnd}
    >
      <SortableContext
        items={sortedTasks.map(task => task.id)}
        strategy={verticalListSortingStrategy}
      >
        <div className="space-y-2">
          {sortedTasks.map(task => (
            <DraggableTaskItem key={task.id} task={task} />
          ))}
        </div>
      </SortableContext>
    </DndContext>
  );
}