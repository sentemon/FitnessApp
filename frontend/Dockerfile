FROM node:20.17.0 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

FROM nginx:1.21.3-alpine
COPY --from=build /app/dist/frontend /usr/share/nginx/html
