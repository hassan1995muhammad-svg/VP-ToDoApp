# Stage 1: Build
FROM node:18-alpine AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy source code
COPY . .

# Build argument for API URL (REQUIRED)
ARG REACT_APP_API_URL

# Validate that REACT_APP_API_URL is provided
RUN if [ -z "$REACT_APP_API_URL" ]; then \
      echo "ERROR: REACT_APP_API_URL build argument is required but not provided!"; \
      exit 1; \
    fi

# Set environment variable
ENV REACT_APP_API_URL=$REACT_APP_API_URL

# Display the API URL being used (for verification)
RUN echo "Building with REACT_APP_API_URL=$REACT_APP_API_URL"

# Build the app
RUN npm run build

# Stage 2: Production with Nginx
FROM nginx:alpine

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built app from build stage
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
