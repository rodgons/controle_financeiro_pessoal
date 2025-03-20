FROM node:22.14-alpine AS builder

WORKDIR /app
COPY package.json pnpm-lock.yaml ./

RUN npm install -g pnpm

ENV PNPM_HOME=/app/.pnpm
ENV PATH=$PNPM_HOME:$PATH

# Install dependencies using pnpm
RUN pnpm install --frozen-lockfile

# Copy the rest of the frontend files
COPY . .

# Build the frontend
RUN pnpm run build

# Stage 2: Serve with Nginx
FROM nginx:stable-alpine AS production
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
