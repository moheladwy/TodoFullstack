import { useAppStore } from "@/lib/store/useStore";
import { TaskList } from "./componenets/TaskList";
import { TaskFilters, SortOption } from "./componenets/TaskFilters";
import { CreateTaskDialog } from "./componenets/CreateTaskDialog";
import Layout from "@/layout";
import { useEffect, useState, useMemo } from "react";
import { useParams, useNavigate } from "react-router";
import { List } from "@/lib/api/interfaces";
import { toast } from "@/hooks/use-toast";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { AppDrawer } from "@/components/app-drawer";
import { EditListForm } from "@/components/edit-list-form";
import { Button } from "@/components/ui/button";
import { MoreHorizontal } from "lucide-react";
import { SidebarTrigger } from "@/components/ui/sidebar";

interface ToastStateProps {
  id: string;
  dismiss: () => void;
}

export function Tasks() {
  const { listId } = useParams()
  const navigate = useNavigate()
  const {
    lists, 
    tasks, 
    setSelectedList, 
    selectedListId,
    selectedListTasks, 
    deleteList, 
    isLoading, 
    error 
  } = useAppStore();
  const [selectedList, setList] = useState<List>();
  const [isEditListOpen, setIsEditListOpen] = useState(false);
  const [search, setSearch] = useState("")
  const [priority, setPriority] = useState("all")
  const [status, setStatus] = useState("all")
  const [sort, setSort] = useState<SortOption>(SortOption.NameAsc)

  // Show loading toast when isLoading changes
  useEffect(() => {
    let toastState: ToastStateProps;
    
    if (isLoading) {
      const { id, dismiss } = toast({
        title: "Loading...",
        description: "Please wait while the action completes.",
        duration: Infinity,
      });
      toastState = { id, dismiss };
    }
    return () => {
      if (toastState?.id) {
        toastState?.dismiss();
      }
    };
  }, [isLoading]);

  useEffect(() => {
    if (error) {
      toast({
        title: "Error",
        description: error,
        variant: "destructive",
      });
    }
  }, [error]);
  
  useEffect(() => {
    if (listId) {
      switch (listId) {
        case 'all':
          setList(undefined)
          break
        case 'completed':
          setList(undefined)
          break
        default:
          {
            const list = lists.find(l => l.id === listId);
            if (list) {
              setList(list)
              setSelectedList(list.id)
            } else {
              navigate('/tasks/all', { replace: true })
            }
          }
      }
    } else if (selectedListId) {
        const list = lists.find(l => l.id === selectedListId);
        if (list) {
          setList(list)
        } else {
          navigate('/tasks/all', { replace: true })
        }
      }
    }, [listId, lists, tasks, navigate, setSelectedList, selectedListId])
  
  const filteredAndSortedTasks = useMemo(() => {
    let filtered = [...selectedListTasks]

    // Apply search filter
    if (search) {
      filtered = filtered.filter(task => 
        task.name.toLowerCase().includes(search.toLowerCase()) ||
        task.description?.toLowerCase().includes(search.toLowerCase())
      );
    }

    // Apply priority filter
    if (priority !== "all") {
      filtered = filtered.filter(task => 
        task.priority === Number(priority)
      )
    }

    // Apply status filter
    if (status !== "all") {
      filtered = filtered.filter(task => 
        status === "completed" ? task.isCompleted : !task.isCompleted
      )
    }

    // Apply sorting
    return filtered.sort((a, b) => {
      switch (sort) {
        case SortOption.NameAsc:
          return a.name.localeCompare(b.name)
        case SortOption.NameDesc:
          return b.name.localeCompare(a.name)
        case SortOption.PriorityHighToLow:
          return a.priority - b.priority
        case SortOption.PriorityLowToHigh:
          return b.priority - a.priority
        default:
          return 0
      }
    })
  }, [selectedListTasks, search, priority, status, sort])

  const handleDeleteList = async () => {
    if (!selectedList) return;
    
    try {
      await deleteList(selectedList.id);
      toast({
        title: "List deleted",
        description: "List and its tasks have been deleted successfully.",
        duration: 2000,
      });
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete list. Please try again.",
        variant: "destructive",
        duration: 5000,
      });
      console.error("Failed to delete list:", error);
    }
  };
  
  // Get the display title based on the current view
  const getViewTitle = () => {
    switch (listId) {
      case 'all':
        return 'All Tasks';
      case 'completed':
        return 'Completed Tasks';
      default:
        return selectedList?.name;
    }
  };

  // Show/hide list actions based on view
  const showListActions = !['all', 'completed'].includes(listId || '');
  
  return (
    <Layout>
      <div className="container mx-auto flex flex-col h-[calc(100vh-2rem)]">
        {/* Sticky Header */}
        <div className="container sticky top-0 z-10 bg-background pt-4 pb-2">
          <div className="flex justify-between items-center mb-6">
            <div className="flex items-center gap-2">
              <div className="flex flex-wrap flex-col justify-between gap-1">
                <div className="flex items-center gap-2">
                  <SidebarTrigger />
                  <h1 className="text-2xl font-bold">{getViewTitle()}</h1>
                  {showListActions && (
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon">
                          <MoreHorizontal className="h-4 w-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => setIsEditListOpen(true)}>
                          Edit List
                        </DropdownMenuItem>
                        <DropdownMenuItem 
                          className="text-destructive"
                          onClick={handleDeleteList}
                        >
                          Delete List
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  )}
                </div>
                {selectedList?.description && (
                  <p className="text-muted-foreground">{selectedList?.description}</p>
                )}
              </div>
            </div>
            {showListActions && <CreateTaskDialog />}
          </div>

          <TaskFilters 
            onSearchChange={setSearch}
            onPriorityChange={setPriority}
            onStatusChange={setStatus}
            onSortChange={setSort}
          />
        </div>

        {/* Scrollable Content */}
        <div className="container flex-1 overflow-auto pb-4">
          <TaskList 
            tasks={filteredAndSortedTasks}
            isCompletedView={listId === 'completed'}
            isAllPendingView={listId === 'all'}
            sortOption={sort}
          />
        </div>

        {/* Edit List Dialog */}
        {showListActions && (
          <AppDrawer 
            open={isEditListOpen} 
            setOpen={setIsEditListOpen}
            title="Edit List"
            description="Make changes to your List here. Click save when you're done."
            form={
              selectedList && (
                <EditListForm 
                  listId={selectedList.id}
                  name={selectedList.name}
                  description={selectedList.description}
                  onClose={() => setIsEditListOpen(false)}
                />
              )
            }
          />
        )}
      </div>
    </Layout>
  );
}
