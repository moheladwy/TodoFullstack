# TodoFullstack

## Introduction

This is a full-stack project that is built with ASP.NET Core 8.0 for the backend, Sqlite3 for the Database, and ReactJS for the front end. The project is a simple to-do list application that allows users to add, delete, and update tasks. The project is built with a RESTful API and uses JWT for authentication.

## Installation

1. Clone the repository using `git clone https://github.com/moheladwy/TodoFullstack.git`
2. Run `cd TodoFullstack` to navigate to the project directory
3. Run `docker build -t todo-fullstack .` to build the docker image
4. Run `docker volume create todo-db` to create a volume for the MongoDB database
5. Run `docker run -p 8070:8080 -v todo-db:/TodoAPI/Database todo-fullstack` to run the docker container
6. The application should now be running on `http://localhost:8070/swagger/index.html` for the API documentation using Swagger.

## Usage

Navigate to `http://localhost:8070/swagger/index.html` to view the API documentation and test the endpoints.

## License

This project is an open-source project that is licensed under the MIT license. You can use this project for personal or educational purposes.

## Author

This project was created by Mohamed Al-Adawy. You can find me on [My Portfolio](https://al-adawy.netlify.app).
