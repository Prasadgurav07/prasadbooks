# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Publish the app
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/prasadbooks/out .

# Expose the port (Render will use this port)
EXPOSE 10000

# Set the environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:10000
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Start the API
ENTRYPOINT ["dotnet", "prasadbooks.dll"]
