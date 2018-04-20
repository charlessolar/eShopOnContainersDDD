#!/bin/bash

docker rm $(docker ps -a -q)
docker rmi $(docker images -q -f dangling=true)

docker-compose -f ../docker-compose.yml -f ../docker-compose.override.yml build
