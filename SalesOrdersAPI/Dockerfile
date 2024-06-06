#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SalesOrdersAPI/SalesOrdersAPI.csproj", "SalesOrdersAPI/"]
RUN dotnet restore "./SalesOrdersAPI/SalesOrdersAPI.csproj"
COPY . .
WORKDIR "/src/SalesOrdersAPI"
RUN dotnet build "./SalesOrdersAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SalesOrdersAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesOrdersAPI.dll"]
