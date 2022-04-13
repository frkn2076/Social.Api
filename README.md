# Social App

<br>

## Features
* ### Authorization
  | Service               | Method        | Endpoint                  |
  | --------------------- |:-------------:| ------------------------- |
  | Register              | POST          | authorization/register    |
  | Login                 | POST          | authorization/login       |
  | Refresh token         | GET           | authorization/refresh     |
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
