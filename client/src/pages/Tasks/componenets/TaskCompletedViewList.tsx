import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion"
import { List, Task } from "@/lib/api/interfaces"
import { useAppStore } from "@/lib/store/useStore"
import { TaskItem } from "./TaskItem"

export default function TaskCompletedViewList() {
  const {lists, tasks} = useAppStore()
  const tasksByList = tasks.reduce((acc, task) => {
    if (task.isCompleted) {
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
    }
    return acc
  }, {} as Record<string, { list: List, tasks: Task[] }>)

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
      {Object.keys(tasksByList).length === 0 && (
        <div className="text-center py-8 text-gray-500">
          No completed tasks found.
        </div>
      )}
    </div>
  )
}