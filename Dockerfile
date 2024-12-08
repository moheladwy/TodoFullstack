# Base image to host the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /Todo
EXPOSE 8080

# Build stage for compiling and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Todo/src

COPY ["Todo.Core/Todo.Core.csproj", "./Todo.Core/"]
RUN dotnet restore "Todo.Core/Todo.Core.csproj"

COPY ["Todo.Infrastructure/Todo.Infrastructure.csproj", "./Todo.Infrastructure/"]
RUN dotnet restore "Todo.Infrastructure/Todo.Infrastructure.csproj"

COPY ["Todo.Api/Todo.Api.csproj", "./Todo.Api/"]
RUN dotnet restore "Todo.Api/Todo.Api.csproj"

COPY Todo.Core/ Todo.Core/
COPY Todo.Infrastructure/ Todo.Infrastructure/
COPY Todo.Api/ Todo.Api/

RUN dotnet build "Todo.Core/Todo.Core.csproj" -c $BUILD_CONFIGURATION -o /Todo.Core/build
RUN dotnet build "Todo.Infrastructure/Todo.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /Todo.Infrastructure/build
RUN dotnet build "Todo.Api/Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.API/build

# Publish stage - create the final publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Todo.Core/Todo.Core.csproj" -c $BUILD_CONFIGURATION -o /Todo.Core/publish
RUN dotnet publish "Todo.Infrastructure/Todo.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /Todo.Infrastructure/publish
RUN dotnet publish "Todo.Api/Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.API/publish /p:UseAppHost=false

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo

RUN mkdir -p Database

COPY --from=publish /Todo.Core/publish .
COPY --from=publish /Todo.Infrastructure/publish .
COPY --from=publish /Todo.API/publish .
COPY --from=build   /Todo/src/Todo.Infrastructure/Migrations Migrations

ENTRYPOINT ["sh", "-c", "dotnet Todo.Api.dll ef database update && dotnet Todo.Api.dll"]
