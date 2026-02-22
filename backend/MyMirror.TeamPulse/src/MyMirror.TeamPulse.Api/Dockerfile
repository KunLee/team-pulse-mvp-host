# Stage 1: base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all csproj files and restore (layered for Docker cache efficiency)
COPY ["src/MyMirror.TeamPulse.Api/MyMirror.TeamPulse.Api.csproj", "src/MyMirror.TeamPulse.Api/"]
COPY ["src/MyMirror.TeamPulse.Application/MyMirror.TeamPulse.Application.csproj", "src/MyMirror.TeamPulse.Application/"]
COPY ["src/MyMirror.TeamPulse.Domain/MyMirror.TeamPulse.Domain.csproj", "src/MyMirror.TeamPulse.Domain/"]
COPY ["src/MyMirror.TeamPulse.Infrastructure/MyMirror.TeamPulse.Infrastructure.csproj", "src/MyMirror.TeamPulse.Infrastructure/"]
RUN dotnet restore "src/MyMirror.TeamPulse.Api/MyMirror.TeamPulse.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/MyMirror.TeamPulse.Api"
RUN dotnet build "MyMirror.TeamPulse.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyMirror.TeamPulse.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMirror.TeamPulse.Api.dll"]
