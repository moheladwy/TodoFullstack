import { Navigate } from 'react-router'
import { useAppStore } from "@/lib/store/useStore"

export function PrivateRoute({ children }: { children: React.ReactNode }) {
  const user = useAppStore().user
  
  if (!user) {
    return <Navigate to="/login" replace />
  }

  return children
}