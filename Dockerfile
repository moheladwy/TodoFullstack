# Base image to host the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /TodoAPI
EXPOSE 8080

# Build stage for compiling and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /TodoAPI/src
COPY ["api/api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY api/ .
RUN dotnet build "api.csproj" -c $BUILD_CONFIGURATION -o /TodoAPI/build

# Publish stage - create the final publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.csproj" -c $BUILD_CONFIGURATION -o /TodoAPI/publish /p:UseAppHost=false

# Final stage with the runtime environment
FROM base AS final
WORKDIR /TodoAPI

RUN mkdir -p /TodoAPI/Migrations /TodoAPI/Database
COPY --from=publish /TodoAPI/publish .
COPY --from=build /TodoAPI/src/Migrations /TodoAPI/Migrations

ENTRYPOINT ["sh", "-c", "dotnet api.dll ef database update && dotnet api.dll"]
