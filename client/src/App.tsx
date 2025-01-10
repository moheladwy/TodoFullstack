import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router'
import { Tasks } from "./pages/Tasks"
import Login from "./pages/Login"
import Register from "./pages/Register"
import { Account } from "./pages/Account"
import { PrivateRoute } from "./components/private-route"
import { Toaster } from "./components/ui/toaster"
import { useAppStore } from './store/useStore'
import { useEffect, useState } from 'react'
import { Loading } from './components/loading'

export default function App() {
  const initializeStore = useAppStore().initializeStore
    const [isInitializing, setIsInitializing] = useState(true)
  
    useEffect(() => {
      const initialize = async () => {
        try {
          await initializeStore()
        } catch (error) {
          console.error('Failed to initialize store:', error)
        } finally {
          setIsInitializing(false)
        }
      }
      initialize()
  }, [initializeStore])
    
    if (isInitializing) {
      return (
        <Loading fullScreen size="lg" />
      );
    }
  
  return (
    <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<Navigate to="/tasks" replace />} />
          <Route
            path="/tasks"
            element={
              <PrivateRoute>
                <Tasks />
              </PrivateRoute>
            }
          >
            <Route index element={<Navigate to="inbox" replace />} />
            <Route path=":listId" element={<Tasks />} />
          </Route>
          <Route
            path="/account"
            element={
              <PrivateRoute>
                <Account />
              </PrivateRoute>
            }
          />
        </Routes>
        <Toaster />
    </Router>
  )
}