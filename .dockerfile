FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

# copy .csproj and restore as distinct layers
COPY "ApiDotNetEngConc/DotnetAPI.sln" "ApiDotNetEngConc/DotnetAPI.sln" 