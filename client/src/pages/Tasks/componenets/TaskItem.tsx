import { Task, TaskPriority } from "@/lib/api/interfaces"
import { useAppStore } from "@/store/useStore"
import { Checkbox } from "@/components/ui/checkbox"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Pencil, Trash2 } from "lucide-react"
import { useState } from "react"
import { EditTaskDialog } from "./EditTaskDialog"
import { toast } from "@/hooks/use-toast"


interface TaskItemProps {
  task: Task
}

const priorityColors = {
  [TaskPriority.Urgent]: "bg-red-500",
  [TaskPriority.High]: "bg-orange-500",
  [TaskPriority.Medium]: "bg-yellow-500",
  [TaskPriority.Low]: "bg-green-500",
}

export function TaskItem({ task }: TaskItemProps) {
  const {updateTask, deleteTask} = useAppStore()
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)

  const handleToggleComplete = async () => {
    await updateTask({
      ...task,
      isCompleted: !task.isCompleted
    })
  }
  
  const handleDeleteTask = async () => {
    try {
      await deleteTask(task.id)
      toast({
        title: "Task deleted",
        description: "Task has been deleted successfully.",
      })
    } catch (error) {
      toast({
        title: "Error",
        description: `Failed to delete task because of: ${error}`,
        variant: "destructive",
      })
    }
  }

  return (
    <>
      <div className="flex items-center gap-4 p-4 bg-white rounded-lg shadow">
        <Checkbox
          checked={task.isCompleted}
          onCheckedChange={handleToggleComplete}
        />
        
        <div className="flex-1">
          <h3 className={`font-medium ${task.isCompleted ? 'line-through text-gray-500' : ''}`}>
            {task.name}
          </h3>
          {task.description && (
            <p className="text-sm text-gray-600">{task.description}</p>
          )}
        </div>

        <div className="flex items-center gap-2">
          <Badge className={priorityColors[task.priority]}>
            {TaskPriority[task.priority]}
          </Badge>
          
          <Button
            variant="ghost"
            size="icon"
            onClick={() => setIsEditDialogOpen(true)}
          >
            <Pencil className="h-4 w-4" />
          </Button>
          
          <Button
            variant="ghost"
            size="icon"
            onClick={handleDeleteTask}
          >
            <Trash2 className="h-4 w-4 text-destructive" />
          </Button>
        </div>
      </div>

      <EditTaskDialog
        task={task}
        open={isEditDialogOpen}
        onOpenChange={setIsEditDialogOpen}
      />
    </>
  )
}