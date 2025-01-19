# Description: Dockerfile for the client side of the application
FROM node:22-alpine AS base
WORKDIR /Todo

# Build stage for compiling and publishing the application
FROM base AS build
WORKDIR /Todo/src

# Add build arguments
ARG VITE_SERVER_URL

# Set environment variables
ENV VITE_SERVER_URL=$VITE_SERVER_URL

COPY package*.json .
RUN npm install
COPY . .
RUN npm run build

# Final stage with the runtime environment
FROM base AS final
WORKDIR /Todo
COPY --from=build /Todo/src/dist .