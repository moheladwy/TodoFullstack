# Base image to host the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /Todo
EXPOSE 8080

# Build stage for compiling and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Todo/src

COPY . .
RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /Todo/build

# Publish stage - create the final publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish -c $BUILD_CONFIGURATION -o /Todo/publish /p:UseAppHost=false

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /Todo/publish .
COPY --from=build   /Todo/src/src/Todo.Infrastructure/Migrations Migrations

ENTRYPOINT ["sh", "-c", "dotnet Todo.Api.dll ef database update && dotnet Todo.Api.dll"]
