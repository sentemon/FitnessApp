FROM node:20.17.0 AS build
WORKDIR /app

RUN npm install -g @angular/cli@18.2.12

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build -- --configuration production

FROM node:20.17.0
WORKDIR /app

RUN npm install -g @angular/cli@18.2.12

COPY --from=build /app/dist /app/dist
COPY package*.json ./

RUN npm install --production

EXPOSE 4200
# Todo: change the way to run
CMD npx http-server ./dist/frontend --port 4200 --host 0.0.0.0
