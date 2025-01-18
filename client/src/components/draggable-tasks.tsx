import {
  DndContext,
  closestCenter,
  KeyboardSensor,
  PointerSensor,
  useSensor,
  useSensors,
  DragEndEvent,
} from '@dnd-kit/core';
import {
  arrayMove,
  SortableContext,
  sortableKeyboardCoordinates,
  verticalListSortingStrategy,
  useSortable,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { useAppStore } from '@/store/useStore';
import { Task, TaskOrder } from '@/lib/api/interfaces';
import { TaskItem } from '@/pages/Tasks/componenets/TaskItem';
import { useMemo } from 'react';

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
}

export function DraggableTasks({ tasks }: DraggableTasksProps) {
  const { selectedListId, taskOrders, reorderTasks } = useAppStore();
  
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 5, // Minimum distance for drag activation
      },
    }),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    })
  );

  const sortedTasks = useMemo(() => {
    if (!selectedListId) return tasks;

    const currentOrders = taskOrders[selectedListId] || [];
    const tasksWithOrder = tasks.map((task, index) => {
      const orderItem = currentOrders.find(order => order.taskId === task.id);
      return {
        ...task,
        order: orderItem?.order ?? index
      };
    });
    return tasksWithOrder.sort((a, b) => a.order - b.order);
  }, [tasks, selectedListId, taskOrders]);

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