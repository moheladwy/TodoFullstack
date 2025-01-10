import { useLocation } from "react-router"
import { SidebarProvider } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

export default function Layout({ children }: { children: React.ReactNode }) {
  const location = useLocation()
  const isAuthRoute = ['/login', '/register'].includes(location.pathname)

  if (isAuthRoute) {
    return children
  }

  return (
    <SidebarProvider defaultOpen={true}>
      <div className="container flex min-h-screen min-w-full">
        <AppSidebar />
        <main className="container mx-auto flex-1">
          {children}
        </main>
      </div>
    </SidebarProvider>
  )
}

