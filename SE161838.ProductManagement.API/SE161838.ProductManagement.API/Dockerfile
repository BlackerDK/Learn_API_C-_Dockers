#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SE161838.ProductManagement.API/SE161838.ProductManagement.API.csproj", "SE161838.ProductManagement.API/"]
COPY ["SE161838.ProductManagement.Repo/SE161838.ProductManagement.Repo.csproj", "SE161838.ProductManagement.Repo/"]
RUN dotnet restore "./SE161838.ProductManagement.API/./SE161838.ProductManagement.API.csproj"
COPY . .
WORKDIR "/src/SE161838.ProductManagement.API"
RUN dotnet build "./SE161838.ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SE161838.ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SE161838.ProductManagement.API.dll"]