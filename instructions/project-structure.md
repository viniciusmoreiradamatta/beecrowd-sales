[Back to README](./README.md)

## Project Structure

The project should be structured as follows:

```
root
├── src/
│ ├── SalesApi/ 		# Sales API code
│ │ ├── Dockerfile 		# Dockerfile for the API
│ │ └── SalesApi.csproj
│ └── Gateway/ 			# Ocelot Gateway code
│   ├── Dockerfile 		# Dockerfile for the Gateway
│   └── ocelot.json 		# Ocelot route configuration file
├── docker-compose.yml 	# Orchestrate all services
```