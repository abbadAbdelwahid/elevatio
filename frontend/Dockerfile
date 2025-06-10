# 🧱 Stage 1: Build the app
FROM node:18-alpine AS builder


# Copy env file explicitly
COPY .env.production .env.production

# Set working directory
WORKDIR /app

# Copy dependency files and install
COPY package*.json ./
RUN npm install

# Copy the rest of the app
COPY . .

# Build the app for production
RUN npm run build

# 🏁 Stage 2: Run the app with minimal image
FROM node:18-alpine AS runner

# Set NODE_ENV to production
ENV NODE_ENV=production

# Create app directory
WORKDIR /app

# Copy built app from builder stage
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

# Expose port
EXPOSE 3000

# Start the app
CMD ["npm", "start"]
