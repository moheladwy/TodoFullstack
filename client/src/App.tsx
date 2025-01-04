import "./App.css";
import { BrowserRouter, Routes, Route, Navigate } from "react-router";
import { AuthProvider } from "./Authentication/context/AuthContext";
import Login from "./Authentication/Login";
import Register from "./Authentication/Register";
import Dashboard from "./Dashboard/Dashboard";
import Navbar from "./Navbar/Navbar";
import ProtectedRoute from "./Authentication/ProtectedRoute";
import { ListsProvider } from "./Dashboard/context/ListsContext";

export default function App() {
	return (
		<BrowserRouter>
			<AuthProvider>
				<ListsProvider>
					<div className="app-container">
						<Navbar />
						<Routes>
							<Route
								index
								element={<Navigate to="/dashboard" replace />}
							/>
							<Route path="/login" element={<Login />} />
							<Route path="/register" element={<Register />} />
							<Route
								path="/dashboard"
								element={
									<ProtectedRoute>
										<Dashboard />
									</ProtectedRoute>
								}
							/>
						</Routes>
					</div>
				</ListsProvider>
			</AuthProvider>
		</BrowserRouter>
	);
}
