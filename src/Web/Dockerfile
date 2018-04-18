FROM alpine as build

RUN apk add --update --no-cache nodejs git
RUN npm i -g yarn webpack webpack-cli

WORKDIR /src

COPY src/Web/package.json .
COPY src/Web/yarn.lock .

RUN yarn

COPY . .
WORKDIR /src/src/Web

ARG API_SERVER

RUN yarn build

FROM nginx:stable as runtime
WORKDIR /usr/share/nginx/html
COPY --from=build /src/src/Web/dist .
