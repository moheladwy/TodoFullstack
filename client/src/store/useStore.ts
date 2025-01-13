import { createStore, useStore } from 'zustand'
import { authApi } from '@/lib/api/calls/AuthCalls'
import { userApi } from '@/lib/api/calls/UserCalls'
import { tasksApi } from '@/lib/api/calls/TasksCalls'
import { listsApi } from '@/lib/api/calls/ListsCalls'
import type { 
  User, 
  Task, 
  List,
  LoginRequest, 
  RegisterRequest, 
  ChangePasswordRequest, 
  UpdateUserInfoRequest, 
  CreateTaskRequest,
  CreateListRequest,
  UpdateListRequest
} from '@/lib/api/interfaces'

interface State {
  user: User | null
  lists: List[]
  selectedListId: string | null
  tasks: Task[]
  selectedListTasks: Task[]
  isLoading: boolean
  error: string | null
  
  // Initialization
  initializeStore: () => Promise<void>
  
  // Auth actions
  login: (credentials: LoginRequest) => Promise<void>
  register: (userInfo: RegisterRequest) => Promise<void>
  logout: () => Promise<void>
  
  // User actions
  fetchUserInfo: (userId: string) => Promise<void>
  changePassword: (passwordData: ChangePasswordRequest) => Promise<void>
  updateUserInfo: (updateData: UpdateUserInfoRequest) => Promise<void>
  deleteAccount: (userId: string) => Promise<void>
  
  // List actions
  fetchLists: () => Promise<void>
  createList: (createListRequest: CreateListRequest) => Promise<void>
  updateList: (updateList: UpdateListRequest) => Promise<void>
  deleteList: (listId: string) => Promise<void>
  setSelectedList: (listId: string) => void
  
  // Task actions
  fetchAllTasks: () => Promise<void>
  setSelectedListTasks: (listId: string) => Promise<void>
  updateTask: (task: Task) => Promise<void>
  createTask: (task: CreateTaskRequest) => Promise<void>
  deleteTask: (taskId: string) => Promise<void>
}

export const appStore = createStore<State>((set, get) => ({
  user: null,
  lists: [],
  selectedListId: null,
  tasks: [],
  selectedListTasks: [],
  isLoading: false,
  error: null,
  
  initializeStore: async () => {
    try {
      const userId = localStorage.getItem('userId')
      const accessToken = localStorage.getItem('accessToken')
      if (!userId || !accessToken) {
        set({ 
          user: null, 
          lists: [], 
          selectedListId: null, 
          tasks: [], 
          isLoading: false 
        })
        return
      }
  
      set({ isLoading: true })
      // Fetch user info
      await get().fetchUserInfo(userId)
      // Fetch lists and tasks
      await get().fetchLists()
      
      set({ isLoading: false })
    } catch (error) {
      // If there's an error (like expired token), clear everything
      console.error('Failed to initialize store:', error)
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      localStorage.removeItem('userId')
      set({ 
        user: null, 
        lists: [], 
        selectedListId: null, 
        tasks: [], 
        isLoading: false,
        error: 'Session expired. Please login again.'
      })
      throw error;
    }
  },

  login: async (credentials) => {
    set({ isLoading: true, error: null })
    try {
      const response = await authApi.login(credentials)
      localStorage.setItem('accessToken', response.accessToken)
      localStorage.setItem('refreshToken', response.refreshToken)
      localStorage.setItem('userId', response.userId)
      await get().fetchUserInfo(response.userId)
      await get().fetchLists()
    } catch (error) {
      set({ error: 'Login failed', isLoading: false })
      throw error
    }
  },

  register: async (userInfo) => {
    set({ isLoading: true, error: null })
    try {
      const response = await authApi.register(userInfo)
      localStorage.setItem('accessToken', response.accessToken)
      localStorage.setItem('refreshToken', response.refreshToken)
      localStorage.setItem('userId', response.userId)
      await get().fetchUserInfo(response.userId)
      await get().fetchLists()
    } catch (error) {
      set({ error: 'Registration failed', isLoading: false })
      throw error
    }
  },

  logout: async () => {
    try {
      set({ isLoading: true, error: null })
      await authApi.logout()
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      localStorage.removeItem('userId')
      set({ user: null, lists: [], selectedListId: null, tasks: [], isLoading: false })
    } catch (error) {
      set({ error: 'Logout failed', isLoading: false })
      throw error
    }
  },

  fetchUserInfo: async (userId) => {
    set({ isLoading: true, error: null })
    try {
      const user = await userApi.getUser(userId)
      set({ user, isLoading: false })
    } catch (error) {
      set({ error: 'Failed to fetch user info', isLoading: false })
      throw error
    }
  },

  changePassword: async (passwordData) => {
    set({ isLoading: true, error: null })
    try {
      await userApi.changePassword(passwordData)
      set({ isLoading: false })
    } catch (error) {
      set({ error: 'Failed to change password', isLoading: false })
      throw error
    }
  },
  
  updateUserInfo: async (updateData) => {
    set({ isLoading: true, error: null })
    try {
      await userApi.updateUserInfo(updateData)
      // After successful update, fetch the updated user info
      const updatedUser = await userApi.getUser(updateData.id)
      set({ 
        user: updatedUser, 
        isLoading: false 
      })
    } catch (error) {
      set({ 
        error: 'Failed to update user information', 
        isLoading: false 
      })
      throw error
    }
  },
  
  deleteAccount: async (userId) => {
    set({ isLoading: true, error: null })
    try {
      await userApi.deleteAccount(userId)
      set({ user: null, lists: [], selectedListId: null, tasks: [], isLoading: false })
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      localStorage.removeItem('userId')
    } catch (error) {
      set({ error: 'Failed to delete account', isLoading: false })
      throw error
    }
  },
  
  fetchLists: async () => {
      set({ isLoading: true, error: null })
      try {
        const lists = await listsApi.getAllLists()
        set({ 
          lists,
          selectedListId: lists.length > 0 ? lists[0].id : null,
          isLoading: false 
        })
      // Fetch all tasks after fetching lists
      await get().fetchAllTasks()
      } catch (error) {
        set({ error: 'Failed to fetch lists', isLoading: false })
        throw error
      }
  },

  createList: async (createListRequest: CreateListRequest) => {
      set({ isLoading: true, error: null })
      try {
        const newList = await listsApi.createList(createListRequest)
        set((state) => ({ 
          lists: [...state.lists, newList],
          selectedListId: newList.id, // Automatically select the new list
          isLoading: false 
        }))
      } catch (error) {
        set({ error: 'Failed to create list', isLoading: false })
        throw error
      }
  },
  
  updateList: async (updateListRequest: UpdateListRequest) => {
    set({ isLoading: true, error: null })
    try {
      const updatedList = await listsApi.updateList(updateListRequest)
      set((state) => ({
        lists: state.lists.map((list) => 
          list.id === updateListRequest.id ? updatedList : list
        ),
        isLoading: false
      }))
    } catch (error) {
      set({ error: 'Failed to update list', isLoading: false })
      throw error
    }
  },
  
  deleteList: async (id: string) => {
    set({ isLoading: true, error: null })
    try {
      await listsApi.deleteList(id)
      set((state) => {
        const newLists = state.lists.filter((list) => list.id !== id)
        return {
          lists: newLists,
          // If the deleted list was selected, select the first available list
          selectedListId: state.selectedListId === id 
            ? (newLists.length > 0 ? newLists[0].id : null)
            : state.selectedListId,
          // Clear tasks if the deleted list was selected
          tasks: state.selectedListId === id ? [] : state.tasks,
          isLoading: false
        }
      })
      
      // If we selected a new list, fetch its tasks
      const newSelectedId = get().selectedListId
      if (newSelectedId) await get().setSelectedListTasks(newSelectedId)
    } catch (error) {
      set({ error: 'Failed to delete list', isLoading: false })
      throw error
    }
  },
  
  setSelectedList: async (listId: string) => {
    const { tasks } = get()
    set({ 
      selectedListId: listId,
      selectedListTasks: tasks.filter(task => task.listId === listId)
    })
    await get().setSelectedListTasks(listId)
  },
  
  fetchAllTasks: async () => {
    set({ isLoading: true, error: null })
    try {
      const { lists, selectedListId } = get()
      if (lists.length === 0) {
        set({ tasks: [], selectedListTasks: [], isLoading: false })
        return
      }
  
      // Fetch tasks for all lists
      const allTasksPromises = lists.map(list => tasksApi.getTasksByListId(list.id))
      const tasksArrays = await Promise.all(allTasksPromises)
      const allTasks = tasksArrays.flat()
  
      // Update state with all tasks and selected list tasks
      set({
        tasks: allTasks,
        selectedListTasks: selectedListId 
          ? allTasks.filter(task => task.listId === selectedListId)
          : [],
        isLoading: false
      })
    } catch (error) {
      set({ error: 'Failed to fetch all tasks', isLoading: false })
      throw error
    }
  },
    
  setSelectedListTasks: async (listId: string) => {
    set({ isLoading: true, error: null })
    try {
      set({ 
        selectedListTasks: get().tasks.filter(task => task.listId === listId),
        isLoading: false
      })
    } catch (error) {
      set({ error: 'Failed to fetch tasks', isLoading: false })
      throw error
    }
  },

  createTask: async (task) => {
    set({ isLoading: true, error: null })
    try {
      const newTask = await tasksApi.createTask(task)
      set((state) => ({
        tasks: [...state.tasks, newTask],
        selectedListTasks: task.listId === state.selectedListId 
          ? [...state.selectedListTasks, newTask]
          : state.selectedListTasks,
        isLoading: false,
      }))
    } catch (error) {
      set({ error: 'Failed to create task', isLoading: false })
      throw error
    }
  },
  
  updateTask: async (task) => {
    set({ isLoading: true, error: null })
    try {
      await tasksApi.updateTask(task)
      set((state) => ({
        tasks: state.tasks.map((t) => (t.id === task.id ? task : t)),
        selectedListTasks: state.selectedListId === task.listId 
          ? state.selectedListTasks.map((t) => (t.id === task.id ? task : t))
          : state.selectedListTasks,
        isLoading: false,
      }))
    } catch (error) {
      set({ error: 'Failed to update task', isLoading: false })
      throw error
    }
  },
  
  deleteTask: async (taskId) => {
    set({ isLoading: true, error: null })
    try {
      await tasksApi.deleteTask(taskId)
      set((state) => ({
        tasks: state.tasks.filter((task) => task.id !== taskId),
        selectedListTasks: state.selectedListTasks.filter((task) => task.id !== taskId),
        isLoading: false,
      }))
    } catch (error) {
      set({ error: 'Failed to delete task', isLoading: false })
      throw error
    }
  },
}))

export function useAppStore() {
  return useStore(appStore)
}