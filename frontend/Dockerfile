ARG ENVIRONMENT=Development

FROM node:20.17.0 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

FROM nginx:1.21.3-alpine

ARG ENVIRONMENT

COPY default.conf /etc/nginx/conf.d/default.conf
COPY prod.conf /etc/nginx/conf.d/prod.conf

RUN if [ "$ENVIRONMENT" = "Production" ]; then \
      cp /etc/nginx/conf.d/prod.conf /etc/nginx/conf.d/default.conf; \
    fi