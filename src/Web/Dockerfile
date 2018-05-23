FROM alpine as build

RUN apk add --update --no-cache nodejs git
RUN npm i -g yarn webpack-cli webpack cross-env

WORKDIR /src

COPY src/Web/package.json .
COPY src/Web/yarn.lock .

RUN yarn

COPY . .
WORKDIR /src/src/Web
RUN yarn build

FROM nginx:stable as runtime
WORKDIR /usr/share/nginx/html
COPY --from=build /src/src/Web/dist .
