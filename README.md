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

<br>

## Databases
 * PostgreSQL

<br>

## Tools
 * Dapper
 * JWT
 * Swagger
 * Docker

## Docker command to run postgres && mongodb locally:
 docker run -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=example -e POSTGRES_DB=my_db -d postgres:13-alpine
 docker run -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=example -d mongo

## Https Certificate
 dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\httpscertificate.pfx -p sEcREtpaSsWord
