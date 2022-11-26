# Social App

<br>

## Features
* ### Authentication
  | Service               | Method        | Endpoint                  |
  | --------------------- |:-------------:| ------------------------- |
  | Register              | POST          | authentication/register   |
  | Login                 | POST          | authentication/login      |
  | Refresh token         | GET           | authentication/refresh    |
* ### Profile
  | Service               | Method        | Endpoint                  |
  | --------------------- |:-------------:| ------------------------- |
  | Get profile by id     | GET           | profile/{id}              |
  | Get user's profile    | GET           | profile/private           |
  | Update user's profile | PUT           | profile/private           |
* ### Activity
  | Service               | Method        | Endpoint                  |
  | --------------------- |:-------------:| ------------------------- |
  | Get activities        | GET           | activity/all/{isRefresh?} |
  | Get user's activities | GET           | profile/private/all       |
  | Update user's profile | PUT           | profile/private           |
* ### Admin
  | Service               | Method        | Endpoint                  |
  | --------------------- |:-------------:| ------------------------- |
  |                       |               |                           |

<br>

## Databases
 * PostgreSQL

<br>

## Tools
 * Dapper
 * JWT
 * Swagger
 * Docker

## Docker command to run postgres locally:
 docker run -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=example -e POSTGRES_DB=my_db -d postgres:13-alpine
