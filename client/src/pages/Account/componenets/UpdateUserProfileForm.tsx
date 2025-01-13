import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { User } from "@/lib/api/interfaces"
import { appStore } from "@/store/useStore"
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { toast } from "@/hooks/use-toast"
import { useState } from "react"

const updateProfileSchema = z.object({
  firstName: z.string()
    .min(3, "First name must be at least 3 characters")
    .max(25, "First name must not exceed 25 characters"),
  lastName: z.string()
    .min(3, "Last name must be at least 3 characters")
    .max(25, "Last name must not exceed 25 characters"),
  userName: z.string()
    .min(3, "Username must be at least 3 characters")
    .max(25, "Username must not exceed 25 characters"),
  email: z.string()
    .email("Invalid email address"),
  phoneNumber: z.string()
    .refine((val) => {
      if (val === "") return true;
      return val.length >= 10 && val.length <= 15;
    }, "Phone number must be between 10 and 15 characters")
    .optional()
    .nullable(),
})

interface UpdateProfileFormProps {
  user: User
}

export function UpdateProfileForm({ user }: UpdateProfileFormProps) {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const form = useForm<z.infer<typeof updateProfileSchema>>({
    resolver: zodResolver(updateProfileSchema),
    defaultValues: {
      firstName: user.firstName,
      lastName: user.lastName,
      userName: user.userName,
      email: user.email,
      phoneNumber: user.phoneNumber ?? "",
    },
  })

  const onSubmit = async (data: z.infer<typeof updateProfileSchema>) => {
    try {
      setIsSubmitting(true)
      await appStore.getState().updateUserInfo({
        id: user.id,
        newFirstName: data.firstName,
        newLastName: data.lastName,
        newUsername: data.userName,
        newEmail: data.email,
        newPhoneNumber: data.phoneNumber || null,
      })

      toast({
        title: "Profile updated",
        description: "Your profile information has been updated successfully.",
      })
    } catch (error: Error | unknown) {
      toast({
        title: "Error",
        description: `Failed to update profile information: because ${error}`,
        variant: "destructive",
      })
      console.error("Failed to update profile information:", error)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Update Profile</CardTitle>
        <CardDescription>
          Make changes to your profile information here.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="firstName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>First Name</FormLabel>
                    <FormControl>
                      <Input 
                        {...field} 
                        placeholder="Enter first name"
                        disabled={isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="lastName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Last Name</FormLabel>
                    <FormControl>
                      <Input 
                        {...field} 
                        placeholder="Enter last name"
                        disabled={isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <FormField
              control={form.control}
              name="userName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Username</FormLabel>
                  <FormControl>
                    <Input 
                      {...field} 
                      placeholder="Enter username"
                      disabled={isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input 
                      {...field} 
                      type="email"
                      placeholder="Enter email address"
                      disabled={isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="phoneNumber"
              render={({ field: { value, ...fieldProps } }) => (
                <FormItem>
                  <FormLabel>Phone Number</FormLabel>
                  <FormControl>
                    <Input 
                      {...fieldProps}
                      value={value ?? ""}
                      type="tel"
                      placeholder="Enter phone number"
                      disabled={isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button type="submit" disabled={isSubmitting}>{isSubmitting ? "Updating..." : "Update Profile"}</Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  )
}
