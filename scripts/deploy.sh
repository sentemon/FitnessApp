#!/bin/bash
set -e

REPO_URL="https://github.com/sentemon/FitnessApp.git"
APP_DIR="/home/FitnessApp"
ENV_FILE="$APP_DIR/.env"

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

# 3. Clone or update the repository
if [ ! -d "$APP_DIR" ]; then
  echo "Cloning repository..."
  sudo git clone $REPO_URL $APP_DIR
else
  echo "Updating repository..."
  cd $APP_DIR
  sudo git reset --hard
  sudo git pull origin master
fi

# 4. Generate .env file if not present
if [ ! -f "$ENV_FILE" ]; then
  echo "Creating .env file..."
  sudo tee "$ENV_FILE" > /dev/null <<EOF
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

# Other env variables can go here...
EOF

  echo ".env file created at $ENV_FILE. Please edit it with real values before starting the app."
  exit 0
fi

# 5. Pull Docker images
echo "Pulling Docker images..."
cd $APP_DIR
sudo docker compose -f docker-compose.prod.yml down
sudo docker compose -f docker-compose.prod.yml pull
echo "Docker images pulled successfully."

# 6. Final instructions
echo ""
echo "To start the application make sure you have edited the .env file, then run this: "
echo ""
echo "sudo docker compose -f docker-compose.prod.yml up -d"