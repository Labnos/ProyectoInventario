# Usa la imagen oficial de ASP.NET Core 8.0 como base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080 # Puerto que Cloud Run espera por defecto
ENV ASPNETCORE_URLS=http://*:8080

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProyectoInventario.csproj", "./"]
RUN dotnet restore "./ProyectoInventario.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ProyectoInventario.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ProyectoInventario.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProyectoInventario.dll"]