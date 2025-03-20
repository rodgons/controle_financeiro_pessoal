# Personal Finance Control System

A full-stack application for managing personal finances, built with .NET Core backend and Vue.js/TypeScript frontend.

## Project Structure

The project is divided into two main components:

### Backend
- Built with .NET Core
- RESTful API architecture
- Docker support for containerization
- Unit tests included

### Frontend
- Vue.js with TypeScript
- Vite as build tool
- Modern UI components
- Docker support for containerization

## Prerequisites

- .NET Core SDK 9
- Node.js (LTS version recommended)
- Docker and Docker Compose (optional, for containerized deployment)
- pnpm (recommended package manager for frontend)

## Getting Started

### Backend Setup

1. Navigate to the Backend directory:
```bash
cd Backend
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

### Frontend Setup

1. Navigate to the Frontend directory:
```bash
cd Frontend
```

2. Install dependencies:
```bash
pnpm install
```

3. Start the development server:
```bash
pnpm dev
```

## Docker Deployment

### Backend
```bash
cd Backend
docker build -t finance-backend .
docker run -p 5000:80 finance-backend
```

### Frontend
```bash
cd Frontend
docker build -t finance-frontend .
docker run -p 3000:80 finance-frontend
```

## Development

- Backend API documentation is available at `/swagger` when running the backend
- Frontend development server runs on `http://localhost:5173` by default
- Backend API runs on `http://localhost:5000` by default

## Testing

### Backend Tests
```bash
cd Backend
dotnet test
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 