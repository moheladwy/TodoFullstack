FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /TodoAPI
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /TodoAPI/src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/TodoAPI/src/"
RUN dotnet build "api.csproj" -c $BUILD_CONFIGURATION -o /TodoAPI/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.csproj" -c $BUILD_CONFIGURATION -o /TodoAPI/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /TodoAPI
COPY --from=publish /TodoAPI/publish .

RUN mkdir -p /TodoAPI/Database
ENTRYPOINT ["dotnet", "api.dll", "ef", "database", "update", "&&", "dotnet", "api.dll"]
