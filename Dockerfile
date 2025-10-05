# ===== Build Stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY prasadbooks.sln ./
COPY prasadbooks/*.csproj ./prasadbooks/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the project
COPY prasadbooks/. ./prasadbooks/

# Publish the project
WORKDIR /app/prasadbooks
RUN dotnet publish -c Release -o out

# ===== Runtime Stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/prasadbooks/out .

# Expose the port Render uses
EXPOSE 10000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:10000
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Start the API
ENTRYPOINT ["dotnet", "prasadbooks.dll"]
