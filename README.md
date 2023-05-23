# UrlShortener C# App + RESTful API

Summary: UrlShortener allows users to manage their own short links! Create, edit, and delete short links of your favourite websites!

- Target platform: .NET 6
- Seeded database with one user and three addresses
- Default user credentials: `guest@mail.com` / `guest123`

## UrlShortener Web App

The ASP.NET Core app "UrlShortener" is an app for making short URLs.

- Technologies: C#, ASP.NET Core, Entity Framework Core, ASP.NET Core Identity, NUnit
- The app supports the following operations:
  - Home page (view latest created / your own short URls): `/`
  - View addresses: `/URLAddress/All`
  - Create a new short URL (URL + short code): `/URLAddress/Add`
  - Edit addresse: `/URLAddress/Edit/:id`
  - Delete addresse: `/URLAddress/Delete/:id`

## UrlShortener RESTful API

The following endpoints are supported:

- `GET /api` - list all API endpoints
- `GET /api/urladdresses` - list all addresses
- `GET /api/urladdresses/count` - returns address count
- `GET /api/urladdresses/search/:keyword` - returns addresses by their original URL
- `POST /api/urladdresses/create` - create a new short URL (send a JSON object in the request body, e.g. `{ "URL": "https://www.google.com", "Short Code": "goog" }`)
- `PUT /api/urladdresses/:originalUrl` - edit address by its original URL (send a JSON object in the request body, holding all fields, e.g. `{ "URL": "https://www.google.com", "Short Code": "goog" }`)
- `DELETE /api/urladdresses/:id` - delete address by `id`
- `POST /api/users/login` - logs in an existing user (send a JSON object in the request body, holding all fields, e.g. `{ "email": "guest@mail.com", "password": "guest123" }`)
- `POST /api/users/register` - registers a new user (send a JSON object in the request body, holding all fields, e.g. `{ "email": "someUsername@mail.bg", "password": "somePassword" }`)

## Screenshots

![home-page](https://user-images.githubusercontent.com/72888249/207355016-1054a001-e68f-4536-93b5-13ebd68c35e3.png)
![register-page](https://user-images.githubusercontent.com/72888249/207467460-24673a99-41c5-49fd-8755-a8ff0d04f4be.png)
![login-page](https://user-images.githubusercontent.com/72888249/207467519-35c3c764-371e-4a9c-9fb7-36d1f53d02c3.png)
![home-page-logged-in](https://user-images.githubusercontent.com/72888249/207356520-32f2bae3-4c02-4445-b6c1-83f66f3a211a.png)
![all-contacts](https://user-images.githubusercontent.com/72888249/207357086-fca28bea-e434-4896-9716-40bb5514889b.png)
![create-contact](https://user-images.githubusercontent.com/72888249/207357475-54646dba-2be6-4d76-81b8-3e05043a1e61.png)
![edit-contact](https://user-images.githubusercontent.com/72888249/207357743-bfa34884-fadf-4794-9fcf-076e7b24baeb.png)
![search-contacts](https://user-images.githubusercontent.com/72888249/207357570-039509a1-ea86-4e79-9e2c-9b5aabae440a.png)
