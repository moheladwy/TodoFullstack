# Description: Dockerfile for the client side of the application
FROM node:20-alpine AS base
WORKDIR /Todo

# Build stage for compiling and publishing the application
FROM base AS build
WORKDIR /Todo/src

# Add build arguments
ARG VITE_SERVER_URL
ARG VITE_SERVER_LOGIN_PATH
ARG VITE_SERVER_REGISTER_PATH
ARG VITE_SERVER_REFRESH_PATH
ARG VITE_SERVER_LOGOUT_PATH

# Set environment variables
ENV VITE_SERVER_URL=$VITE_SERVER_URL
ENV VITE_SERVER_LOGIN_PATH=$VITE_SERVER_LOGIN_PATH
ENV VITE_SERVER_REGISTER_PATH=$VITE_SERVER_REGISTER_PATH
ENV VITE_SERVER_REFRESH_PATH=$VITE_SERVER_REFRESH_PATH
ENV VITE_SERVER_LOGOUT_PATH=$VITE_SERVER_LOGOUT_PATH

COPY package*.json .
RUN npm install
COPY . .
RUN npm run build

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo
COPY --from=build /Todo/src/dist .