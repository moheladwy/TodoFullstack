FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /Todo.Api
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Todo.Api.csproj", "./"]
RUN dotnet restore "Todo.Api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.Api/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Todo.Api.csproj" -c $BUILD_CONFIGURATION -o /Todo.Api/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /Todo.Api

RUN mkdir -p /Todo.Api/Database
COPY --from=publish /Todo.Api/publish .
ENTRYPOINT ["dotnet", "api.dll", "ef", "database", "update", "dotnet", "api.dll"]
