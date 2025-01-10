import { useState } from "react";
import { UseLists } from "../context/ListsContext";

export default function Sidebar() {
	const {
		toggleSidebar,
		lists,
		selectedList,
		setSelectedList,
		addList,
		deleteList,
	} = UseLists();
	const [newListName, setNewListName] = useState<string>("");
	const [error, setError] = useState<string | null>(null);

	const handleAddList = async (e: React.FormEvent) => {
		e.preventDefault();
		if (!newListName.trim()) return;

		try {
			addList(newListName);
			setNewListName("");
		} catch (err) {
			setError("Failed to create list");
			console.error(err);
		}
	};

	const handleDeleteList = async (listId: string) => {
		try {
			if (!window.confirm("Are you sure you want to delete this list?")) {
				return;
			}
			deleteList(listId);
		} catch (err) {
			setError("Failed to delete list");
			console.error(err);
		}
	};

	return (
		<aside className="col-md-3 col-lg-2 d-md-block bg-dark text-light sidebar t-smooth">
			<div className="position-sticky h-100 overflow-auto">
				<div className="p-3">
					<div className="d-flex justify-content-between align-items-center mb-3">
						<h5 className="fs-3 mb-0">All Lists</h5>
						<i
							className="bi bi-layout-sidebar-inset fs-4 cursor-pointer"
							onClick={toggleSidebar}
						></i>
					</div>

					{/* Divider */}
					<div className="border-bottom mb-3"></div>
					<form onSubmit={handleAddList} className="mb-3">
						<div className="input-group">
							<input
								type="text"
								placeholder="New list name..."
								className="form-control"
								value={newListName}
								onChange={(e) => setNewListName(e.target.value)}
							/>
							<button type="submit" className="btn btn-primary">
								Add
							</button>
						</div>
					</form>
					{error && (
						<div className="alert alert-danger mt-3">{error}</div>
					)}
					<div className="list-group">
						{lists.map((list) => (
							<a
								key={list.id}
								className={`hover list-group-item list-group-item-action d-flex justify-content-between align-items-center ${
									selectedList?.id === list.id
										? "selected"
										: "not-selected"
								} text-light cursor-pointer`}
								onClick={() => {
									if (selectedList?.id !== list.id)
										setSelectedList(list);
								}}
							>
								<span>{list.name}</span>
								<button
									onClick={(e) => {
										e.stopPropagation();
										handleDeleteList(list.id);
									}}
									className="btn btn-outline-danger btn-sm"
								>
									<i className="bi bi-trash-fill"></i>
								</button>
							</a>
						))}
					</div>
				</div>
			</div>
		</aside>
	);
}
