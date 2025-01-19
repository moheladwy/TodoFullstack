import { List, Task } from "@/lib/api/interfaces";
import { useAppStore } from "@/lib/store/useStore";
import { useEffect } from "react";
import { TaskItem } from "./TaskItem";
import NoPendingTasks from "@/components/no-pending-tasks";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion";

export default function AllPendingTasksList() {
  const { lists, tasks } = useAppStore()
  const tasksByList = tasks.reduce((acc, task) => {
    if (task.isCompleted) return acc
    const list = lists.find(l => l.id === task.listId)
    if (list) {
      if (!acc[list.id]) {
        acc[list.id] = {
          list,
          tasks: []
        }
      }
      acc[list.id].tasks.push(task)
    }
    return acc
  }, {} as Record<string, { list: List, tasks: Task[] }>)
  
  useEffect(() => {
  }, [tasks])
  
  if (Object.keys(tasksByList).length === 0) 
    return <NoPendingTasks />
  
  return (
    <div className="space-y-4">
      {Object.values(tasksByList).map(({ list, tasks }) => (
        tasks.length > 0 && ( // Only render if there are completed tasks for this list
          <Accordion key={list.id} type="single" collapsible>
            <AccordionItem value={list.id}>
              <AccordionTrigger>
                {list.name} ({tasks.length})
              </AccordionTrigger>
              <AccordionContent>
                <div className="space-y-2">
                  {tasks.map(task => (
                    <TaskItem key={task.id} task={task} />
                  ))}
                </div>
              </AccordionContent>
            </AccordionItem>
          </Accordion>
        )
      ))}
    </div>
  )
}