import { Calendar, CheckCircle, ChevronUp, ListTodo, LogOut, User2, Plus, X } from "lucide-react"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuBadge,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarSeparator,
} from "@/components/ui/sidebar"
import { 
  DropdownMenu, 
  DropdownMenuContent, 
  DropdownMenuItem, 
  DropdownMenuTrigger 
} from "@/components/ui/dropdown-menu"
import { useAppStore } from "@/store/useStore"
import { Navigate, useNavigate, useParams } from "react-router"
import { useMemo, useState } from "react"
import { Input } from "@/components/ui/input"
import { toast } from "@/hooks/use-toast"
import { Button } from "./ui/button"
import { ThemeToggle } from "@/components/theme-toggle"
import { DraggableLists } from "./draggable-lists"

export function AppSidebar() {
  const navigate = useNavigate()
  const { listId } = useParams()
  const { user, logout, tasks, createList } = useAppStore()
  const [isAddingList, setIsAddingList] = useState(false)
  const [newListName, setNewListName] = useState("")

  // Calculate pending and completed tasks
  const { pendingTasksCount, totalCompletedTasks } = useMemo(() => {
    const pending = new Map<string, number>()
    let completed = 0
    
    tasks.forEach(task => {
      if (task.isCompleted) {
        completed++
      } else {
        const currentCount = pending.get(task.listId) || 0
        pending.set(task.listId, currentCount + 1)
      }
    })
    
    return {
      pendingTasksCount: pending,
      totalCompletedTasks: completed
    }
  }, [tasks])

  // Calculate total pending tasks
  const totalPendingTasks = useMemo(() => {
    return Array.from(pendingTasksCount.values()).reduce((a, b) => a + b, 0)
  }, [pendingTasksCount])
  
  const handleSelectList = (listId: string) => {
    navigate(`/tasks/${listId}`, { replace: true })
  }
  
  const handleCreateList = async (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && newListName.trim()) {
      try {
        await createList({
          name: newListName.trim(),
          description: "",
          userId: user?.id || "",
        })
        setNewListName("")
        setIsAddingList(false)
      } catch (error) {
        toast({
          title: "Error",
          description: "Failed to create list. Please try again.",
          variant: "destructive",
        })
        console.error(error);
      }
    }
  }
  
  const handleLogout = async () => {
    try {
      await logout()
      navigate("/login", { replace: true })
    } catch (error) {
      console.error("Logout failed:", error)
    }
  }

  if (!user) return <Navigate to="/login" replace />

  return (
    <Sidebar>
      {/* Sidebar Header */}
      <SidebarHeader>
        <h1 className="text-3xl font-bold text-center">Tasker</h1>
      </SidebarHeader>

      {/* Sidebar Content */}
      <SidebarContent>
        {/* Pinned Lists */}
        <SidebarGroup>
          <SidebarGroupLabel>Pinned</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {/* All Tasks */}
              <SidebarMenuItem>
                <SidebarMenuButton 
                  asChild 
                  onClick={() => handleSelectList('all')}
                  isActive={listId === 'all'}
                >
                  <button className="w-full flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <ListTodo className="h-4 w-4" />
                      <span>All Tasks</span>
                    </div>
                    {totalPendingTasks > 0 && (
                      <SidebarMenuBadge>
                        {totalPendingTasks}
                      </SidebarMenuBadge>
                    )}
                  </button>
                </SidebarMenuButton>
              </SidebarMenuItem>

              {/* Today's Tasks */}
              <SidebarMenuItem>
                <SidebarMenuButton 
                  asChild 
                  disabled
                >
                  <button className="w-full flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Calendar className="h-4 w-4" />
                      <span>Today</span>
                    </div>
                    <SidebarMenuBadge>
                      Soon
                    </SidebarMenuBadge>
                  </button>
                </SidebarMenuButton>
              </SidebarMenuItem>

              {/* Completed Tasks */}
              <SidebarMenuItem>
                <SidebarMenuButton 
                  asChild 
                  onClick={() => handleSelectList('completed')}
                  isActive={listId === 'completed'}
                >
                  <button className="w-full flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <CheckCircle className="h-4 w-4" />
                      <span>Completed</span>
                    </div>
                    {totalCompletedTasks > 0 && (
                      <SidebarMenuBadge>
                        {totalCompletedTasks}
                      </SidebarMenuBadge>
                    )}
                  </button>
                </SidebarMenuButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>

        <SidebarSeparator />
        
        <SidebarGroup>
          <div className="flex items-center justify-between px-0">
            <SidebarGroupLabel>Add new list</SidebarGroupLabel>
            <Button
              variant="ghost"
              size="icon"
              onClick={() => setIsAddingList(!isAddingList)}
            >
              {isAddingList ? (
                <X className="h-4 w-4" />
              ) : (
                <Plus className="h-4 w-4" />
              )}
            </Button>
          </div>
          {isAddingList && (
            <div className="px-2 py-2">
              <Input
                placeholder="New list name..."
                value={newListName}
                onChange={(e) => setNewListName(e.target.value)}
                onKeyDown={handleCreateList}
                autoFocus
              />
            </div>
          )}
          {/* User Lists */}
          <SidebarGroupContent>
            <SidebarMenu>
              {/* {lists.map((list) => {
                const pendingCount = pendingTasksCount.get(list.id) || 0
                
                return (
                  <SidebarMenuItem key={list.id}>
                    <SidebarMenuButton 
                      asChild 
                      onClick={() => handleSelectList(list.id)}
                      isActive={listId === list.id}
                    >
                      <button className="w-full flex items-center justify-between">
                        <span>{list.name}</span>
                        {pendingCount > 0 && (
                          <SidebarMenuBadge>
                            {pendingCount}
                          </SidebarMenuBadge>
                        )}
                      </button>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                )
              })} */}
              <DraggableLists />
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      
      {/* Sidebar Footer */}
      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <div className="flex items-center justify-between px-2 py-2">
              <ThemeToggle /> {/* Add the ThemeToggle component here */}
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <SidebarMenuButton>
                    <User2 />
                    <span>{user.userName}</span>
                    <ChevronUp className="ml-auto" />
                  </SidebarMenuButton>
                </DropdownMenuTrigger>
                <DropdownMenuContent
                  side="top"
                  className="w-[--radix-popper-anchor-width]"
                >
                  <DropdownMenuItem onClick={() => navigate("/account")}>
                    <User2 className="mr-2 h-4 w-4" />
                    <span>Account</span>
                  </DropdownMenuItem>
                  <DropdownMenuItem className="text-destructive" onClick={handleLogout}>
                    <LogOut className="mr-2 h-4 w-4 text-destructive" />
                    <span className="text-destructive">Sign out</span>
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </div>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  )
}