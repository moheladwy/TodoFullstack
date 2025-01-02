import { Link, useNavigate } from "react-router";
import { UseAuth } from "../Authentication/context/AuthContext";

export default function Navbar() {
	const { isAuthenticated, logout } = UseAuth();
	const navigate = useNavigate();

	const handleLogout = () => {
		logout();
		navigate("/login");
	};

	return (
		<nav
			className="navbar navbar-expand-lg navbar-dark py-0 h-10"
			style={{ backgroundColor: "rgb(24, 26, 28)" }}
		>
			<div className="container-fluid mx-5">
				<Link className="navbar-brand fs-1 fw-bold mx-5" to="/">
					Tasker
				</Link>
				<button
					className="navbar-toggler"
					type="button"
					data-bs-toggle="collapse"
					data-bs-target="#navbarNav"
				>
					<span className="navbar-toggler-icon"></span>
				</button>
				<div className="collapse navbar-collapse" id="navbarNav">
					<ul className="navbar-nav ms-auto">
						{isAuthenticated ? (
							<>
								<li className="nav-item me-2">
									<Link
										className="btn btn-primary"
										to="/dashboard"
									>
										Dashboard
									</Link>
								</li>
								<li className="nav-item">
									<button
										onClick={handleLogout}
										className="btn btn-danger"
									>
										Logout
									</button>
								</li>
							</>
						) : (
							<>
								<li className="nav-item">
									<Link className="nav-link" to="/login">
										Login
									</Link>
								</li>
								<li className="nav-item">
									<Link className="nav-link" to="/register">
										Register
									</Link>
								</li>
							</>
						)}
					</ul>
				</div>
			</div>
		</nav>
	);
}
