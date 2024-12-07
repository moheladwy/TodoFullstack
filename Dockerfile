# Base image to host the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /Todo.API
EXPOSE 8080

# Build stage for compiling and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Todo.API/src
COPY ["Todo.Api/Todo.Api.csproj", "./"]
RUN dotnet restore "Todo.Api.csproj"
COPY Todo.Api/ .
RUN dotnet build "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.API/build

# Publish stage - create the final publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.API/publish /p:UseAppHost=false

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo.API

RUN mkdir -p /Todo.API/Migrations /Todo.API/Database
COPY --from=publish /Todo.API/publish .
COPY --from=build /Todo.API/src/Migrations /Todo.API/Migrations

ENTRYPOINT ["sh", "-c", "dotnet api.dll ef database update && dotnet api.dll"]
