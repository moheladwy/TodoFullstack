FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /TodoApi
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "api.csproj" -c $BUILD_CONFIGURATION -o /TodoApi/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api.csproj" -c $BUILD_CONFIGURATION -o /TodoApi/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /TodoApi

RUN mkdir -p /TodoApi/Database
COPY --from=publish /TodoApi/publish .
ENTRYPOINT ["dotnet", "api.dll", "ef", "database", "update", "dotnet", "api.dll"]
