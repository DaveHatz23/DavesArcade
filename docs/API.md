# 📡 Dave's Arcade API Documentation

Complete API reference for the Dave's Arcade Game Catalog API.

## Base URL

```
https://localhost:7xxx
```

*(Port number varies - check console output when running `dotnet run`)*

---

## Table of Contents

- [Authentication](#authentication)
- [Error Handling](#error-handling)
- [Available Genres](#available-genres)
- [Endpoints](#endpoints)
  - [Get All Games](#get-all-games)
  - [Get Game by ID](#get-game-by-id)
  - [Create Game](#create-game)
  - [Update Game](#update-game)
  - [Delete Game](#delete-game)
- [Models](#models)
- [Status Codes](#status-codes)

---

## Authentication

Currently, this API does not require authentication. All endpoints are publicly accessible.

> 🔐 **Future Enhancement**: JWT-based authentication will be added in Phase 3 of the roadmap.

---

## Error Handling

The API uses a consistent error response format across all endpoints:

### Error Response Structure

```json
{
  "code": "string",
  "error": "string"
}
```

### Error Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `code` | string | Machine-readable error code (e.g., `Game.NotFound`) |
| `error` | string | Human-readable error message |

### Example Error Responses

**404 Not Found**
```json
{
  "code": "Game.NotFound",
  "error": "Game with ID 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d' was not found."
}
```

**400 Bad Request (Validation Error)**
```json
{
  "code": "Game.InvalidGenre",
  "error": "Genre with ID 'invalid-guid' does not exist."
}
```

**409 Conflict**
```json
{
  "code": "Game.Duplicate",
  "error": "A game with this name already exists."
}
```

---

## Available Genres

The following genre IDs are currently supported:

| Genre ID | Genre Name |
|----------|------------|
| `11111111-1111-1111-1111-111111111111` | Shooter |
| `22222222-2222-2222-2222-222222222222` | RPG |
| `33333333-3333-3333-3333-333333333333` | Simulation |
| `44444444-4444-4444-4444-444444444444` | Adventure |

> **Note**: Using an invalid genre ID will result in a `400 Bad Request` with error code `Game.InvalidGenre`.

---

## Endpoints

### Get All Games

Retrieves a list of all games in the catalog.

**Endpoint**
```
GET /games
```

**Request**

No parameters required.

**Response**

- **Status Code**: `200 OK`
- **Content-Type**: `application/json`

**Response Body**

```json
[
  {
    "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
    "name": "Halo Infinite",
    "genre": "Shooter",
    "price": 59.99,
    "releaseDate": "2021-12-08",
    "imageUri": "/images/halo-infinite.jpg",
    "lastUpdatedBy": "admin"
  },
  {
    "id": "b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e",
    "name": "Elden Ring",
    "genre": "RPG",
    "price": 69.99,
    "releaseDate": "2022-02-25",
    "imageUri": "/images/elden-ring.jpg",
    "lastUpdatedBy": "admin"
  },
  {
    "id": "c3d4e5f6-a7b8-4c5d-9e1f-3a4b5c6d7e8f",
    "name": "Stardew Valley",
    "genre": "Simulation",
    "price": 14.99,
    "releaseDate": "2016-02-26",
    "imageUri": "/images/stardew-valley.jpg",
    "lastUpdatedBy": "admin"
  },
  {
    "id": "d4e5f6a7-b8c9-4d5e-9f1a-4b5c6d7e8f9a",
    "name": "Cyberpunk 2077",
    "genre": "RPG",
    "price": 49.99,
    "releaseDate": "2020-12-10",
    "imageUri": "/images/cyberpunk-2077.jpg",
    "lastUpdatedBy": "admin"
  },
  {
    "id": "e5f6a7b8-c9d1-4e5f-9a1b-5c6d7e8f9a0b",
    "name": "The Legend of Zelda: Tears of the Kingdom",
    "genre": "Adventure",
    "price": 69.99,
    "releaseDate": "2023-05-12",
    "imageUri": "/images/zelda-totk.jpg",
    "lastUpdatedBy": "admin"
  }
]
```

**Example**

```bash
curl -X GET "https://localhost:7001/games" -H "accept: application/json"
```

---

### Get Game by ID

Retrieves a single game by its unique identifier.

**Endpoint**
```
GET /games/{id}
```

**Path Parameters**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | GUID | Yes | Unique identifier of the game |

**Response**

- **Status Code**: `200 OK` (Success) / `404 Not Found` (Game doesn't exist)
- **Content-Type**: `application/json`

**Success Response (200)**

```json
{
  "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
  "name": "Halo Infinite",
  "genre": "Shooter",
  "price": 59.99,
  "releaseDate": "2021-12-08",
  "imageUri": "/images/halo-infinite.jpg",
  "lastUpdatedBy": "admin"
}
```

**Error Response (404)**

```json
{
  "code": "Game.NotFound",
  "error": "Game with ID 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d' was not found."
}
```

**Example**

```bash
curl -X GET "https://localhost:7001/games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d" \
  -H "accept: application/json"
```

---

### Create Game

Creates a new game in the catalog.

**Endpoint**
```
POST /games
```

**Request Headers**

| Header | Value |
|--------|-------|
| `Content-Type` | `application/json` |

**Request Body**

```json
{
  "name": "Starfield",
  "genreId": "22222222-2222-2222-2222-222222222222",
  "price": 69.99,
  "releaseDate": "2023-09-06",
  "description": "An open-world RPG set in space with unprecedented freedom to explore.",
  "imageUri": "/images/starfield.jpg",
  "lastUpdatedBy": "admin"
}
```

**Request Body Fields**

| Field | Type | Required | Constraints | Description |
|-------|------|----------|-------------|-------------|
| `name` | string | Yes | - | Name of the game |
| `genreId` | GUID | Yes | Must be valid genre | Genre identifier |
| `price` | decimal | Yes | - | Price in USD |
| `releaseDate` | date | Yes | ISO 8601 format | Game release date |
| `description` | string | Yes | - | Game description |
| `imageUri` | string | No | Default: null | URL or path to game image |
| `lastUpdatedBy` | string | No | Default: "system" | User who created the game |

**Response**

- **Status Code**: `201 Created` (Success) / `400 Bad Request` (Validation error)
- **Content-Type**: `application/json`
- **Location Header**: `/games/{newGameId}` (on success)

**Success Response (201)**

```json
{
  "id": "f1e2d3c4-b5a6-4d5e-8f7a-9b8c7d6e5f4a",
  "name": "Starfield",
  "genre": "RPG",
  "price": 69.99,
  "releaseDate": "2023-09-06",
  "imageUri": "/images/starfield.jpg",
  "lastUpdatedBy": "admin"
}
```

**Error Response (400)**

```json
{
  "code": "Game.InvalidGenre",
  "error": "Genre with ID '12345678-1234-1234-1234-123456789012' does not exist."
}
```

**Example**

```bash
curl -X POST "https://localhost:7001/games" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Starfield",
    "genreId": "22222222-2222-2222-2222-222222222222",
    "price": 69.99,
    "releaseDate": "2023-09-06",
    "description": "An open-world RPG set in space.",
    "imageUri": "/images/starfield.jpg",
    "lastUpdatedBy": "admin"
  }'
```

---

### Update Game

Updates an existing game in the catalog.

**Endpoint**
```
PUT /games/{id}
```

**Path Parameters**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | GUID | Yes | Unique identifier of the game to update |

**Request Headers**

| Header | Value |
|--------|-------|
| `Content-Type` | `application/json` |

**Request Body**

```json
{
  "name": "Halo Infinite (Updated)",
  "genreId": "11111111-1111-1111-1111-111111111111",
  "price": 49.99,
  "releaseDate": "2021-12-08",
  "description": "Updated description with new DLC information."
}
```

**Request Body Fields**

| Field | Type | Required | Constraints | Description |
|-------|------|----------|-------------|-------------|
| `name` | string | Yes | Max 50 characters | Updated name |
| `genreId` | GUID | Yes | Must be valid genre | Updated genre identifier |
| `price` | decimal | Yes | 1-100 | Updated price in USD |
| `releaseDate` | date | Yes | ISO 8601 format | Updated release date |
| `description` | string | Yes | Max 500 characters | Updated description |

> **Note**: `imageUri` is not updated through this endpoint. The original `imageUri` is preserved. `lastUpdatedBy` is automatically set to "system".

**Response**

- **Status Code**: `200 OK` (Success) / `404 Not Found` (Game doesn't exist) / `400 Bad Request` (Validation error)
- **Content-Type**: `application/json`

**Success Response (200)**

```json
{
  "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
  "name": "Halo Infinite (Updated)",
  "genre": "Shooter",
  "price": 49.99,
  "releaseDate": "2021-12-08",
  "imageUri": "/images/halo-infinite.jpg",
  "lastUpdatedBy": "system"
}
```

**Error Response (404)**

```json
{
  "code": "Game.NotFound",
  "error": "Game with ID 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d' was not found."
}
```

**Error Response (400)**

```json
{
  "code": "Game.InvalidGenre",
  "error": "Genre with ID '12345678-1234-1234-1234-123456789012' does not exist."
}
```

**Example**

```bash
curl -X PUT "https://localhost:7001/games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Halo Infinite (Updated)",
    "genreId": "11111111-1111-1111-1111-111111111111",
    "price": 49.99,
    "releaseDate": "2021-12-08",
    "description": "Updated description."
  }'
```

---

### Delete Game

Deletes a game from the catalog.

**Endpoint**
```
DELETE /games/{id}
```

**Path Parameters**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | GUID | Yes | Unique identifier of the game to delete |

**Response**

- **Status Code**: `204 No Content` (Success) / `404 Not Found` (Game doesn't exist)

**Success Response (204)**

No content returned.

**Error Response (404)**

```json
{
  "code": "Game.NotFound",
  "error": "Game with ID 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d' was not found."
}
```

**Example**

```bash
curl -X DELETE "https://localhost:7001/games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d" \
  -H "accept: application/json"
```

---

## Models

### GameResultDto

Represents a game entity returned by the API.

| Field | Type | Description |
|-------|------|-------------|
| `id` | GUID | Unique identifier of the game |
| `name` | string | Name of the game |
| `genre` | string | Genre name (e.g., "Shooter", "RPG") |
| `price` | decimal | Price in USD |
| `releaseDate` | date | Release date in ISO 8601 format (YYYY-MM-DD) |
| `imageUri` | string | URL or path to the game's image |
| `lastUpdatedBy` | string | User who last updated the game |

**Example**

```json
{
  "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
  "name": "Halo Infinite",
  "genre": "Shooter",
  "price": 59.99,
  "releaseDate": "2021-12-08",
  "imageUri": "/images/halo-infinite.jpg",
  "lastUpdatedBy": "admin"
}
```

### CreateGameRequest

Request model for creating a new game.

| Field | Type | Required | Default | Constraints |
|-------|------|----------|---------|-------------|
| `name` | string | Yes | - | - |
| `genreId` | GUID | Yes | - | Must be valid genre |
| `price` | decimal | Yes | - | - |
| `releaseDate` | date | Yes | - | ISO 8601 format |
| `description` | string | Yes | - | - |
| `imageUri` | string | No | null | - |
| `lastUpdatedBy` | string | No | "system" | - |

**Example**

```json
{
  "name": "Starfield",
  "genreId": "22222222-2222-2222-2222-222222222222",
  "price": 69.99,
  "releaseDate": "2023-09-06",
  "description": "An open-world RPG set in space.",
  "imageUri": "/images/starfield.jpg",
  "lastUpdatedBy": "admin"
}
```

### UpdateGameRequest

Request model for updating an existing game.

| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| `name` | string | Yes | Max 50 characters |
| `genreId` | GUID | Yes | Must be valid genre |
| `price` | decimal | Yes | 1-100 |
| `releaseDate` | date | Yes | ISO 8601 format |
| `description` | string | Yes | Max 500 characters |

**Example**

```json
{
  "name": "Halo Infinite (Updated)",
  "genreId": "11111111-1111-1111-1111-111111111111",
  "price": 49.99,
  "releaseDate": "2021-12-08",
  "description": "Updated description with new DLC information."
}
```

---

## Status Codes

The API uses standard HTTP status codes:

| Status Code | Description |
|-------------|-------------|
| `200 OK` | Request succeeded |
| `201 Created` | Resource created successfully |
| `204 No Content` | Request succeeded with no content to return |
| `400 Bad Request` | Invalid request (validation error) |
| `404 Not Found` | Resource not found |
| `409 Conflict` | Conflict with existing resource |
| `500 Internal Server Error` | Unexpected server error (handled by global exception handler) |

---

## Testing the API

### Using Swagger UI

1. Run the API: 
   ```bash
   cd src/DavesArcade.Api
   dotnet run
   ```
2. Navigate to `https://localhost:7xxx/swagger` (check console for actual port)
3. Use the interactive UI to test endpoints

### Using cURL

See the examples provided for each endpoint above.

### Using Postman

1. Import the OpenAPI specification from `https://localhost:7xxx/swagger/v1/swagger.json`
2. Postman will automatically generate a collection with all endpoints

### Using .NET HTTP Client

```csharp
using System.Net.Http.Json;

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7001") };

// Get all games
var games = await client.GetFromJsonAsync<List<GameResultDto>>("/games");

// Get game by ID
var game = await client.GetFromJsonAsync<GameResultDto>("/games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d");

// Create game
var newGame = new CreateGameRequest(
    "Starfield",
    Guid.Parse("22222222-2222-2222-2222-222222222222"),
    69.99m,
    new DateOnly(2023, 9, 6),
    "An open-world RPG set in space."
);
var response = await client.PostAsJsonAsync("/games", newGame);
```

---

## Rate Limiting

Currently, no rate limiting is implemented. This will be added in Phase 2 of the roadmap.

**Planned implementation:**
- 100 requests per minute per IP address
- `X-RateLimit-Remaining` header in responses

---

## Pagination

Currently, `GET /games` returns all games without pagination. Pagination support will be added in Phase 2.

**Planned pagination parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 10, max: 100)

**Planned response format:**
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalCount": 50
}
```

---

## Filtering & Sorting

Not currently implemented. Planned for Phase 2.

**Planned query parameters:**
- `genre` - Filter by genre name
- `minPrice` / `maxPrice` - Price range filter
- `name` - Search by game name (partial match)
- `sortBy` - Sort field (name, price, releaseDate)
- `sortOrder` - asc/desc

**Example:**
```
GET /games?genre=RPG&minPrice=20&maxPrice=60&sortBy=price&sortOrder=asc
```

---

## Versioning

Currently using v1 (no version prefix in URL). API versioning will be added in Phase 2.

**Planned format:** `/api/v1/games`, `/api/v2/games`

---

## Global Exception Handling

All unhandled exceptions are caught by the `GlobalExceptionHandler` middleware and returned as:

```json
{
  "error": "An internal server error occurred.",
  "message": "Exception message here",
  "timestamp": "2026-04-03T15:30:00Z"
}
```

**Status Code:** `500 Internal Server Error`

---

## CORS

Currently, CORS is not configured. This will be added when a frontend is integrated.

**Planned configuration:**
- Allow specific origins
- Allow all HTTP methods
- Allow credentials

---

## Support

For questions or issues:
- Open an issue on [GitHub](https://github.com/DaveHatz23/DavesArcade/issues)
- Contact: [@DaveHatz23](https://github.com/DaveHatz23)

---

**Last Updated:** April 2026  
**API Version:** 1.0  
**Documentation Version:** 1.0

