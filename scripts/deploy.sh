#!/bin/bash
set -e

REPO_URL="https://github.com/sentemon/FitnessApp.git"
APP_DIR="/home/FitnessApp"

# 1. Update system and install dependencies
sudo apt update && apt upgrade -y
sudo apt install -y git curl

# 2. Install Docker if not present
if ! command -v docker &> /dev/null; then
  curl -fsSL https://get.docker.com -o get-docker.sh
  sudo sh get-docker.sh
  sudo usermod -aG docker $USER
  rm get-docker.sh
fi

# 3. Clone the repository (or update it)
if [ ! -d "$APP_DIR" ]; then
  sudo git clone $REPO_URL $APP_DIR
else
  cd $APP_DIR
  sudo git reset --hard
  sudo git pull origin main
fi

# 4. Start Docker Compose
cd $APP_DIR
docker compose down
docker compose pull
docker compose up -d --build

echo "Deployment completed!"
