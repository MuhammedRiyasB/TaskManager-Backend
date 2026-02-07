# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY TaskManager.Api/*.csproj ./TaskManager.Api/
COPY TaskManager.Modules.Auth/*.csproj ./TaskManager.Modules.Auth/
COPY TaskManager.Modules.Tasks/*.csproj ./TaskManager.Modules.Tasks/
COPY TaskManager.Shared/*.csproj ./TaskManager.Shared/   
RUN dotnet restore

COPY . .
WORKDIR /app/TaskManager.Api
RUN dotnet publish -c Release -o /out

# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
