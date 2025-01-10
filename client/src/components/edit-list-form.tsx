import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { useAppStore } from "@/store/useStore"
import { Button } from "@/components/ui/button"
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
import { toast } from "@/hooks/use-toast"

const editListSchema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string().optional(),
})

interface EditListFormProps {
  listId: string
  name: string
  description?: string
  onClose: () => void
}

export function EditListForm({ 
  listId, 
  name, 
  description, 
  onClose 
}: EditListFormProps) {
  const updateList = useAppStore().updateList
  
  const form = useForm<z.infer<typeof editListSchema>>({
    resolver: zodResolver(editListSchema),
    defaultValues: {
      name,
      description: description || "",
    },
  })

  const onSubmit = async (data: z.infer<typeof editListSchema>) => {
    try {
      await updateList(listId, data.name, data.description)
      toast({
        title: "List updated",
        description: "Your list has been updated successfully.",
        variant: "default",
      })
      onClose()
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to update list. Please try again.",
        variant: "destructive",
      })
      console.error('Failed to update list:', error)
    }
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input {...field} />
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
                <Textarea {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex justify-end gap-2">
          <Button type="button" variant="outline" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit">
            Save changes
          </Button>
        </div>
      </form>
    </Form>
  )
}