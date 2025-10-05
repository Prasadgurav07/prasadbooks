# ===== Build Stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY prasadbooks.sln ./
COPY prasadbooks/ ./prasadbooks/

# Restore dependencies
WORKDIR /src/prasadbooks
RUN dotnet restore

# Publish the project
RUN dotnet publish -c Release -o /app/out

# ===== Runtime Stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/out .

# Expose Render port dynamically
ARG PORT
ENV ASPNETCORE_URLS=http://*:${PORT:-10000}
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE ${PORT:-10000}

# Start the API
ENTRYPOINT ["dotnet", "prasadbooks.dll"]
