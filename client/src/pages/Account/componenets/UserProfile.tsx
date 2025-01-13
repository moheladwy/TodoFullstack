import { User } from "@/lib/api/interfaces"
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { useNavigate } from "react-router"
import { useAppStore } from "@/store/useStore"
import { useState } from "react"
import { toast } from "@/hooks/use-toast"
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"

interface UserProfileProps {
  user: User
}

export function UserProfile({ user }: UserProfileProps) {
  const navigate = useNavigate()
  const { deleteAccount } = useAppStore()
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)
  const getInitials = (firstName: string, lastName: string) => {
    return `${firstName[0]}${lastName[0]}`.toUpperCase()
  }
  
  const handleDeleteAccount = async () => {
    try {
      await deleteAccount(user.id)
      toast({
        title: "Account deleted",
        description: "Your account has been deleted successfully.",
      })
      navigate("/register", { replace: true })
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete account. Please try again.",
        variant: "destructive",
      })
      console.error("Failed to delete account:", error)
    }
  }

  return (
    <Card className="flex flex-col">
      <CardHeader className="pb-4">
        <CardTitle>Profile Information</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="flex items-center gap-4">
          <Avatar className="h-16 w-16">
            <AvatarFallback className="text-lg">
              {getInitials(user.firstName, user.lastName)}
            </AvatarFallback>
          </Avatar>
          
          <div> 
            <h2 className="text-xl font-semibold">
              {user.firstName} {user.lastName}
            </h2>
            <p className="text-sm text-muted-foreground">@{user.userName}</p>
          </div>
        </div>

        <div className="grid gap-3">
          <div>
            <label className="text-sm font-medium text-muted-foreground">Email</label>
            <p className="mt-1">{user.email}</p>
          </div>
          
          <div>
            <label className="text-sm font-medium text-muted-foreground">Phone Number</label>
            <p className="mt-1">{user.phoneNumber || 'Not provided'}</p>
          </div>
        </div>
      </CardContent>
      <CardFooter className="border-t pt-6">
        <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
          <DialogTrigger asChild>
            <Button variant="destructive" className="w-full">
              Delete Account
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Delete Account</DialogTitle>
              <DialogDescription>
                Are you sure you want to delete your account? This action cannot be undone.
              </DialogDescription>
            </DialogHeader>
            <div className="flex justify-end gap-2">
              <Button variant="outline" onClick={() => setIsDeleteDialogOpen(false)}>
                Cancel
              </Button>
              <Button variant="destructive" onClick={handleDeleteAccount}>
                Delete Account
              </Button>
            </div>
          </DialogContent>
        </Dialog>
      </CardFooter>
    </Card>
  )
}