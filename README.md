# Link Shortener

Simple link shortener with:
- .NET Minimal API backend + Redis storage
- React + Vite frontend
- optional password-protected short links

## Project structure

- `LinkShortener.API` - backend solution
  - `LinkShortener.API/LinkShortener.API` - API app
  - `LinkShortener.API/LinkShortener.API.Tests` - tests
- `LinkShortener.Frontend/linkshortener` - React frontend

## Requirements

- .NET SDK 10 (project targets `net10.0`)
- Node.js + npm
- Redis running locally (default: `localhost:6379`)

## Backend setup

1. Open `LinkShortener.API/LinkShortener.API/appsettings.Development.json`
2. Set values:
   - `userIdentifier` (default: `1234`)
   - `ConnectionStrings.RedisConnection` (Redis connection string)

Run API:

```bash
cd LinkShortener.API/LinkShortener.API
dotnet run
```

By default API listens on:
- `https://localhost:7162`
- `http://localhost:5088`

## Frontend setup

1. Open frontend folder:

```bash
cd LinkShortener.Frontend/linkshortener
```

2. Install dependencies:

```bash
npm install
```

3. Configure `.env`:

```env
VITE_API_URL=http://localhost:5088
```

Optional:
- `VITE_ROUTER_BASENAME=/` (or `/link` when deployed under subpath)

4. Run frontend:

```bash
npm run dev
```

Frontend is available on `http://localhost:5173`.

## Usage

### Create short link

On homepage:
- fill `URL`, `SHORT URL`, optional `LINK PASSWORD`, and `USER IDENTIFIER`
- submit form

### Open short link

Open:

```text
http://localhost:5173/<short-code>
```

If link is password-protected, UI asks for password before redirect.

## Notes

- If destination URL is entered without protocol (e.g. `google.com`), frontend normalizes it to `https://google.com`.
- Frontend has API fallback for local development (`https://localhost:7162` and `http://localhost:5088`) to reduce local certificate issues.
