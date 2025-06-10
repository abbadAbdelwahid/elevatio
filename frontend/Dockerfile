# 🧱 Stage 1: Build the app
FROM node:18-alpine AS builder


# Accept build-time environment variables
ARG NEXT_PUBLIC_API_AUTH_URL
ARG NEXT_PUBLIC_API_COURSE_URL
ARG NEXT_PUBLIC_API_EVALUATION_URL
ARG NEXT_PUBLIC_API_ANALYTICS_URL

# Set those ARGs as ENV so Next.js (or React) sees them during `npm run build`
ENV NEXT_PUBLIC_API_AUTH_URL=$NEXT_PUBLIC_API_AUTH_URL
ENV NEXT_PUBLIC_API_COURSE_URL=$NEXT_PUBLIC_API_COURSE_URL
ENV NEXT_PUBLIC_API_EVALUATION_URL=$NEXT_PUBLIC_API_EVALUATION_URL
ENV NEXT_PUBLIC_API_ANALYTICS_URL=$NEXT_PUBLIC_API_ANALYTICS_URL

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
