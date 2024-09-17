FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Copy các dự án và csproj
COPY ["Polaby.API/Polaby.API.csproj", "Polaby.API/"]
COPY ["Polaby.Services/Polaby.Services.csproj", "Polaby.Services/"]
COPY ["Polaby.Repositories/Polaby.Repositories.csproj", "Polaby.Repositories/"]
RUN dotnet restore Polaby.API/Polaby.API.csproj

# Copy toàn bộ mã nguồn
COPY . .

# Chuyển đến thư mục API để build
WORKDIR "/Polaby.API"
RUN dotnet build Polaby.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish dự án
FROM build AS publish
WORKDIR /Polaby.API
RUN dotnet publish Polaby.API.csproj -c $BUILD_CONFIGURATION -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PORT=8080
ENTRYPOINT ["dotnet", "Polaby.API.dll"]
