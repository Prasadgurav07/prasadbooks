# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY prasadbooks.sln ./
COPY prasadbooks/ ./prasadbooks/

WORKDIR /src/prasadbooks
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ARG PORT
ENV ASPNETCORE_URLS=http://*:${PORT:-10000}
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE ${PORT:-10000}

ENTRYPOINT ["dotnet", "prasadbooks.dll"]
