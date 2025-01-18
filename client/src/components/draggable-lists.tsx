import { useMemo } from 'react';
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
import { List, ListOrder } from '@/lib/api/interfaces';
import { SidebarMenuItem, SidebarMenuButton, SidebarMenuBadge } from './ui/sidebar';
import { useNavigate } from 'react-router';

interface DraggableListItemProps {
  list: List;
  order: number;
  onClick: () => void;
  isActive: boolean;
  pendingCount: number;
}

function DraggableListItem({ list, onClick, isActive, pendingCount }: DraggableListItemProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: list.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div ref={setNodeRef} style={style}>
      <SidebarMenuItem>
        <SidebarMenuButton
          asChild
          isActive={isActive}
          className="relative w-full"
        >
          <button
            onClick={onClick}
            className="w-full flex items-center justify-between"
            {...attributes}
            {...listeners}
          >
            <span>{list.name}</span>
            {pendingCount > 0 && (
              <SidebarMenuBadge>
                {pendingCount}
              </SidebarMenuBadge>
            )}
          </button>
        </SidebarMenuButton>
      </SidebarMenuItem>
    </div>
  );
}

export function DraggableLists() {
  const navigate = useNavigate();
  const { lists, selectedListId, setSelectedList, 
    listOrder, reorderLists, tasks } = useAppStore();
  
  const pendingTasksCount = useMemo(() => {
    const pending = new Map<string, number>();
    tasks.forEach(task => {
      if (!task.isCompleted) {
        const currentCount = pending.get(task.listId) || 0;
        pending.set(task.listId, currentCount + 1);
      }
    });
    return pending;
  }, [tasks]);
  
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

  const sortedLists = useMemo(() => {
    const listsWithOrder = lists.map((list, index) => {
      const orderItem = listOrder.find(order => order.listId === list.id);
      return {
        ...list,
        order: orderItem?.order ?? index
      };
    });
    return listsWithOrder.sort((a, b) => a.order - b.order);
  }, [lists, listOrder]);

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    const oldIndex = sortedLists.findIndex(list => list.id === active.id);
    const newIndex = sortedLists.findIndex(list => list.id === over.id);

    const newSortedLists = arrayMove(sortedLists, oldIndex, newIndex);
    const newOrder: ListOrder[] = newSortedLists.map((list, index) => ({
      listId: list.id,
      order: index,
    }));

    reorderLists(newOrder);
  };
  
  const handleSelectList = (listId: string) => {
    setSelectedList(listId);
    navigate(`/tasks/${listId}`, { replace: true });
  };

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragEnd={handleDragEnd}
    >
      <SortableContext
        items={sortedLists.map(list => list.id)}
        strategy={verticalListSortingStrategy}
      >
        {sortedLists.map(list => (
          <DraggableListItem
            key={list.id}
            list={list}
            order={list.order}
            onClick={() => handleSelectList(list.id)}
            isActive={selectedListId === list.id}
            pendingCount={pendingTasksCount.get(list.id) || 0}
          />
        ))}
      </SortableContext>
    </DndContext>
  );
}