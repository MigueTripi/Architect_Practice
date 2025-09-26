#!/bin/bash
set -e

CONFIGURATION=${1:-Debug}
ENVIRONMENT=${2:-Development}
export ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

echo "Building images and loading services with configuration $CONFIGURATION and environment $ENVIRONMENT..."
docker compose build --build-arg BUILD_CONFIGURATION=$CONFIGURATION usermanagement-webapi
docker compose up usermanagement-webapi -d

echo "Applying DB migrations..."
# In case of we want to run the migrations from inside the container, we should dedicate a specific container for that or increase the webapi container's resources.
# docker compose exec webapi dotnet ef database update --project src/SelfResearch.UserManagement.API/SelfResearch.UserManagement.API.csproj
dotnet ef database update --project src/SelfResearch.UserManagement.API/SelfResearch.UserManagement.API.csproj --startup-project src/SelfResearch.UserManagement.API/SelfResearch.UserManagement.API.csproj

# echo "Building Digital Wallet site $CONFIGURATION and environment $ENVIRONMENT..."
# docker compose build  digitalwallet-web
docker compose build digitalwallet-web
docker compose up digitalwallet-web -d 


echo "API local/Docker: http://localhost:5266"
echo "Web local/Docker: http://localhost:4222"
echo "Nginx reverse proxy: http://localhost:8081"
