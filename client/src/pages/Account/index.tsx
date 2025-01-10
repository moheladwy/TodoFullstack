import { useAppStore } from "@/store/useStore"
import { UserProfile } from "./componenets/UserProfile"
import { ChangePasswordForm } from "./componenets/ChangePasswordForm"
import { UpdateProfileForm } from "./componenets/UpdateUserProfileForm"
import { Navigate } from "react-router"
import Layout from "@/layout"
import { SidebarTrigger } from "@/components/ui/sidebar"

export function Account() {
  const { user } = useAppStore()

  if (!user) return <Navigate to="/login" replace />

  return (
    <Layout>
      <div className="mx-auto p-4 space-y-6">
        <div className="flex flex-row flex-wrap gap-4 justify-items-center direction-normal">
          <SidebarTrigger />
          <h1 className="text-3xl font-bold mb-6">Profile Settings</h1>
        </div>
        <div className="grid gap-6 md:grid-cols-2">
          <UserProfile user={user} />
          
          <div className="space-y-6">
            <UpdateProfileForm user={user} />
            <ChangePasswordForm userId={user.id} />
          </div>
        </div>
      </div>
    </Layout>
  )
}