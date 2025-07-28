#!/bin/bash

cd /home/ubuntu/FitnessApp || exit 1
docker compose -f docker-compose.prod.yml pull