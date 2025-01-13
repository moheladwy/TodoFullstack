import { useForm } from "react-hook-form"
import { Task, TaskPriority } from "@/lib/api/interfaces"
import { useAppStore } from "@/store/useStore"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { toast } from "@/hooks/use-toast"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { useState } from "react"

const taskSchema = z.object({
  name: z.string()
    .min(1, "Name must be at least 1 character")
    .max(100, "Name must not exceed 100 characters"),
  description: z.string()
    .max(500, "Description must not exceed 500 characters")
    .optional(),
  priority: z.number()
    .min(0, "Priority must be between 0 and 4")
    .max(4, "Priority must be between 0 and 4"),
  isCompleted: z.boolean(),
});

type TaskForm = z.infer<typeof taskSchema>;

interface EditTaskDialogProps {
  task: Task
  open: boolean
  onOpenChange: (open: boolean) => void
}

const taskStatus = [
  { label: "Completed", value: true },
  { label: "Pending", value: false },
] as const

export function EditTaskDialog({ task, open, onOpenChange }: EditTaskDialogProps) {
  const updateTask = useAppStore().updateTask
  const [isSubmitting, setIsSubmitting] = useState(false)
  const form = useForm({
    resolver: zodResolver(taskSchema),
    defaultValues: {
      name: task.name,
      description: task.description || "",
      priority: task.priority,
      isCompleted: task.isCompleted,
    },
  })

  const onSubmit = async (data: TaskForm) => {
    try {
      setIsSubmitting(true)
      await updateTask({
        ...task,
        ...data,
      })
      toast({
        title: "Task updated",
        description: "Task has been updated successfully.",
        duration: 3000
      })
      onOpenChange(false)
    } catch (error) {
      toast({
        title: "Error",
        description: `Failed to update task because of ${error}`,
        duration: 6000,
        variant: "destructive",
      })
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit Task</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Task Name</FormLabel>
                  <FormControl>
                    <Input {...field} disabled={isSubmitting} placeholder="Enter task name" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            
            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description</FormLabel>
                  <FormControl>
                    <Textarea 
                      {...field} 
                      placeholder="Enter task description (optional)"
                      disabled={isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            
            <FormField
              control={form.control}
              name="priority"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Priority</FormLabel>
                  <Select
                    onValueChange={(value) => field.onChange(Number(value))}
                    defaultValue={field.value.toString()}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Select priority" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {Object.entries(TaskPriority)
                        .filter(([key]) => isNaN(Number(key)))
                        .map(([key, value]) => (
                          <SelectItem key={value} value={value.toString()}>
                            {key}
                          </SelectItem>
                        ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />
            
            <FormField
              control={form.control}
              name="isCompleted"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Status</FormLabel>
                  <Select
                    onValueChange={(value) => field.onChange(value === "true")}
                    defaultValue={field.value.toString()}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Select status" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {taskStatus.map((status) => (
                        <SelectItem
                          key={status.value.toString()}
                          value={status.value.toString()}
                        >
                          {status.label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />
            
            <div className="flex justify-end gap-2">
              <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={isSubmitting}>{isSubmitting ? "Saving..." : "Save changes"}</Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}