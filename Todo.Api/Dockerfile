FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /Todo.Api
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "api.csproj" -c $BUILD_CONFIGURATION -o /Todo.Api/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.csproj" -c $BUILD_CONFIGURATION -o /Todo.Api/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /Todo.Api

RUN mkdir -p /Todo.Api/Database
COPY --from=publish /Todo.Api/publish .
ENTRYPOINT ["dotnet", "api.dll", "ef", "database", "update", "dotnet", "api.dll"]
