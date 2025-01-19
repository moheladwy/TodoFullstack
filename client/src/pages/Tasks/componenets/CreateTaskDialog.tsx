import { useState } from "react"
import { useForm } from "react-hook-form"
import { TaskPriority } from "@/lib/api/interfaces"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
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
import { useAppStore } from "@/lib/store/useStore"
import { toast } from "@/hooks/use-toast"
import * as z from "zod"
import { zodResolver } from "@hookform/resolvers/zod" 

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
});

type CreateTaskForm = z.infer<typeof taskSchema>;

export function CreateTaskDialog() {
  const [open, setOpen] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const form = useForm<CreateTaskForm>({
    resolver: zodResolver(taskSchema),
    defaultValues: {
      name: "",
      description: "",
      priority: TaskPriority.Low,
    },
  })
  const { createTask, selectedListId } = useAppStore()

  const onSubmit = async (data: CreateTaskForm) => {
    try {
      if (!selectedListId) {
        toast({
          title: "Error",
          description: "No list selected. Please select a list first.",
          duration: 5000,
          variant: "destructive",
        })
        return
      }
      setIsSubmitting(true)
      // Here you would call your API to create the task
      await createTask({
        name: data.name,
        description: data.description || "",
        priority: data.priority,
        isCompleted: false,
        listId: selectedListId,
      })
      toast({
        title: "Task created",
        description: "New task has been created successfully.",
        duration: 2000,
      })
      setOpen(false)
      form.reset()
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to create task. Please try again.",
        variant: "destructive",
        duration: 5000,
      })
      console.error('Failed to create task:', error)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>Create New Task</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Task</DialogTitle>
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
                    <Input {...field} placeholder="Enter Task Name"/>
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
                    <Textarea {...field} placeholder="Enter task description (optional)"/>
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
            
            <div className="flex justify-end gap-2">
              <Button type="button" variant="outline" onClick={() => setOpen(false)}>
                Cancel
              </Button>
              <Button type="submit">{isSubmitting ? "Creating..." : "Create Task"}</Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}