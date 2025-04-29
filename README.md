# Project Overview

This project is a simple API developed with .NET 8 to manage a sales context.

## Running the Project

You can run the project using:
- `docker-compose up` 
- Or directly from Visual Studio or any IDE of your choice.

## Gateway

- Address: `http://[your-host]:7777`
- Swagger UI: `http://[your-host]:7777/swagger`

Before starting the project, ensure the network is created:
```bash
docker network create -d bridge my-bridge-network
```

## Messaging

RabbitMQ was chosen using the standard .NET client to keep the setup simple and save time, as I haven't used Rebus before.

To reduce complexity and development effort, the workers are implemented inside the same API project. While the implementation is minimal, it's structured in a way that can be easily extended with business logic.

You can access the RabbitMQ management UI:
- Address: `http://[your-host]:15672`
  - **User:** `userMaster`
  - **Password:** `p455w0rd`

## Metrics

### Prometheus
Prometheus collects and stores API metrics.
- Access at: `http://localhost:9090`

### Grafana
Grafana visualizes the API metrics.
- Access at: `http://localhost:3000`
- Credentials:
  - **User:** `admin`
  - **Password:** `172839`

## Cache

Redis is used to cache some requests. This is a basic example that can be further improved with more time.

