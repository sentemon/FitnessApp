FROM node:20.17.0 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build -- --configuration production

FROM node:20.17.0
WORKDIR /app

COPY --from=build /app/dist /app/dist
COPY package*.json ./

RUN npm install --production

EXPOSE 4000

CMD ["npm", "run", "serve:ssr:frontend"]
