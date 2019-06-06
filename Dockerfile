FROM microsoft/dotnet:sdk AS build
# FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:sdk AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "e-commerce.webapi.dll"]
