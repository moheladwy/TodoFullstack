# TodoFullstack

Todo Fullstack is a comprehensive task management application that showcases the integration between ASP.NET Core 8 Web API and React (Typescript). The application allows users to create, update, and delete task lists and tasks with priorities. It also supports user authentication and authorization with JWT tokens and refresh tokens.

## Motivation

As a software developer passionate about full-stack development, I created this project to strengthen my expertise in:

-   Building production-grade REST APIs with ASP.NET Core 8.0
-   Implementing secure authentication and authorization
-   Developing modern frontend applications with React and TypeScript
-   Working with databases, caching, and containerization
-   Following industry best practices and design patterns
-   Writing clean, maintainable, and testable code

This project serves as a practical demonstration of integrating multiple modern technologies while adhering to software engineering principles.

## Technologies

### Backend

-   ASP.NET Core 8.0
-   Entity Framework Core with SQLite
-   Redis for caching
-   JWT Authentication
-   Swagger API documentation
-   Docker containerization

### Frontend

-   React 18
-   TypeScript
-   Vite
-   Bootstrap 5
-   Axios for API communication

## Features

-   User authentication (register, login, logout)
-   JWT token management with refresh tokens
-   Task lists management (CRUD operations)
-   Tasks management with priorities (CRUD operations)
-   Responsive design
-   Real-time task status updates
-   Caching for improved performance

## Prerequisites

-   Docker and Docker Compose
-   Node.js 20+ (for local development)
-   .NET 8.0 SDK (for local development)

## Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/moheladwy/TodoFullstack.git
    cd TodoFullstack
    ```

2. Development (local) Setup:

    For Backend:

    ```sh
    cd server
    dotnet restore
    dotnet build
    dotnet run --launch-profile https --project Todo.Api/Todo.Api.csproj
    ```

    For Frontend:

    ```sh
    cd client
    npm install
    npm run dev
    ```

3. Docker Setup (Production):
    ```sh
    docker-compose -f docker-compose.yml up -d
    ```

## Environment Variables

### Backend (appsettings.json)

```json
{
	"ConnectionStrings": {
		"SqliteConnection": "Data Source=Database/todo.db",
		"RedisConnection": "localhost:6379"
	},
	"JwtConfigurations": {
		"SecretKey": "your-secret-key",
		"Issuer": "Todo.Api",
		"Audience": "http://localhost:5076",
		"AccessTokenExpirationDays": 30,
		"RefreshTokenExpirationDays": 90
	}
}
```

### Frontend (.env)

```
VITE_SERVER_URL=http://localhost:8070/api
VITE_SERVER_LOGIN_PATH=Auth/login
VITE_SERVER_REGISTER_PATH=Auth/register
VITE_SERVER_REFRESH_PATH=Auth/refresh
VITE_SERVER_LOGOUT_PATH=Auth/logout
```

## API Documentation

When running, access the Swagger documentation at:

-   Development: `http://localhost:5076/swagger`
-   Production: `http://localhost:8070/swagger`

## Project Structure

```
├── client/                   # React frontend
│   ├── src/
│   │   ├── API/              # API integration
│   │   ├── Authentication/   # Auth components
│   │   ├── Dashboard/        # Main app components
│   │   └── Navbar/           # Navigation components
│   └── ...
├── server/                   # ASP.NET Core backend
│   ├── Todo.Api/             # API endpoints
│   ├── Todo.Core/            # Domain models
│   ├── Todo.Infrastructure/  # Data access & services
│   └── Todo.UnitTests/       # Unit tests
├── docker-compose.yml
└── README.md
```

## Development

1. API Development:

-   Use Visual Studio or VS Code
-   Run `dotnet watch` in the Todo.Api directory
-   Access the API at `http://localhost:5076`

2. Frontend Development:

-   Use VS Code
-   Run `npm run dev` in the client directory
-   Access the app at `http://localhost:5173`

## Deployment

Deploy the application:

```sh
docker-compose -f docker-compose.yml up --build -d
```

The application will be available at:

-   Frontend: `http://localhost:80`
-   Backend API: `http://localhost:8070`

## License

This project is licensed under the MIT License - see the [LICENSE](/LICENSE) for details.

## Author

Created by Mohamed Al-Adawy

-   [Portfolio](https://al-adawy.netlify.app)
-   [LinkedIn](https://www.linkedin.com/in/mohamed-al-adawy)
-   [Email](mailto:mohamed.h.eladwy@gmail.com)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## Support

For support, contact the author or open an issue on GitHub.
