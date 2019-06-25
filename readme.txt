notes

docker run --name publisher_api --env ASPNETCORE_ENVIRONMENT=Development -p 8080:80 publisher_api:latest
use container names to reference other containers within the same docker network
