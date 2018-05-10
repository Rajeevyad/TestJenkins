Write-Host "Stopping all container ......................"
docker stop $(docker ps -aq)

Write-Host "Removing all container ......................"
docker rm $(docker ps -aq)

Write-Host "Removing previous built images......................"
docker-compose -f docker-compose.yml -f docker-compose.override.yml down

Write-Host "Check config of container......................"
docker-compose -f docker-compose.yml -f docker-compose.override.yml config

Write-Host "Build and run docker container........."
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build -d

Write-Host "Tagging Image........."
docker tag testjenkins:latest rajeevyad/testjenkins:latest

