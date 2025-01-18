import { useEffect} from "react"
import { TaskItem } from "./TaskItem"
import { Task } from "@/lib/api/interfaces"
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion"
import TaskCompletedViewList from "./TaskCompletedViewList"
import AllPendingTasksList from "./AllPendingTasksList"
import NoPendingTasks from "@/components/no-pending-tasks"
import { DraggableTasks } from "@/components/draggable-tasks"

interface TaskListProps {
  tasks: Task[]
  isCompletedView?: boolean
  isAllPendingView?: boolean
}

export function TaskList({ tasks, isCompletedView, isAllPendingView }: TaskListProps) {
  const pendingTasks = tasks.filter(task => !task.isCompleted)
  const completedTasks = tasks.filter(task => task.isCompleted)
  
  useEffect(() => {
    console.log('TaskList rendered')
  }, [tasks])
  
  if (isCompletedView) return <TaskCompletedViewList />
  if (isAllPendingView) return <AllPendingTasksList />
  if (tasks.length === 0) return <NoPendingTasks />

  return (
    <div className="container space-y-4">
        <div className="container space-y-2">
          <DraggableTasks tasks={pendingTasks} />
        </div>
      <Accordion type="multiple" className="space-y-1 border-0">
        {completedTasks.length > 0 && (
          <AccordionItem value="completed">
            <AccordionTrigger className="px-4 hover:no-underline">
              <div className="container flex items-center">
                <span>Completed Tasks</span>
                <span className="ml-2 inline-flex h-5 w-5 items-center justify-center rounded-full bg-green-100 text-xs text-green-600">
                  {completedTasks.length}
                </span>
              </div>
            </AccordionTrigger>
            <AccordionContent className="px-2">
              <div className="container space-y-1">
                {completedTasks.map((task) => (
                  <TaskItem key={task.id} task={task} />
                ))}
              </div>
            </AccordionContent>
          </AccordionItem>
        )}
      </Accordion>

      {/* Show message when no pending tasks */}
      {pendingTasks.length === 0 && (
        <NoPendingTasks />
      )}
    </div>
  )
}