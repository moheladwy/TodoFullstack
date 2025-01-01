import { Navigate, useLocation } from "react-router";
import { UseAuth } from "./context/AuthContext";

export default function ProtectedRoute({
	children,
}: {
	children: React.ReactNode;
}) {
	const { isAuthenticated } = UseAuth();
	const location = useLocation();

	return isAuthenticated ? (
		children
	) : (
		<Navigate to="/login" state={{ from: location }} replace />
	);
}
