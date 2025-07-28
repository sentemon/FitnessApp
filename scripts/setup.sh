#!/bin/bash

ENV_FILE="/home/ubuntu/FitnessApp/.env"

# 1. Update system and install dependencies
echo "Updating system and installing dependencies..."
sudo apt update && sudo apt upgrade -y
sudo apt install -y git curl

# 2. Install Docker if not present
if ! command -v docker &> /dev/null; then
  echo "Installing Docker..."
  curl -fsSL https://get.docker.com -o get-docker.sh
  sudo sh get-docker.sh
  sudo usermod -aG docker $USER
  rm get-docker.sh
  echo "Docker installed. Please log out and log in again to apply group changes."
fi

# 3. Generate .env file if not present
echo "Creating .env file at $ENV_FILE..."

echo "
# Basic
LETSENCRYPT_EMAIL=

# PostgreSQL
POSTGRES_USER=
POSTGRES_PASSWORD=

AUTH_DATABASE_CONNECTION_STRING=
FILE_DATABASE_CONNECTION_STRING=
POST_DATABASE_CONNECTION_STRING=
WORKOUT_DATABASE_CONNECTION_STRING=
CHAT_DATABASE_CONNECTION_STRING=

# Azure Blob
AZURE_STORAGE_CONNECTION_STRING=
AZURITE_ACCOUNTS=

# RabbitMQ
RABBITMQ_DEFAULT_USER=
RABBITMQ_DEFAULT_PASS=

# Keycloak
KEYCLOAK_ADMIN_USERNAME=
KEYCLOAK_ADMIN_PASSWORD=
" > .env

echo ".env file created. Please edit it with your values."

# 4. How to start the application
echo "You don't need to start the application manually. The CI/CD pipeline will handle it for you."
echo "Otherwise, read README.md for manual instructions: https://github.com/sentemon/FitnessApp/develop/README.md"