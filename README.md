<h1 align="center">NeerCore ↗⭐↗</h1>

--------------------------------

<div align="center">

[![NuGet Release](https://img.shields.io/nuget/v/NeerCore?label=Actual&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages?q=NeerCore)
[![NuGet Pre-Release](https://img.shields.io/nuget/dt/NeerCore.DependencyInjection?color=512bd4&style=for-the-badge)](https://www.nuget.org/packages/NeerCore.DependencyInjection)

[//]: # ([![NuGet Pre-Release]&#40;https://img.shields.io/nuget/vpre/NeerCore?label=Latest&logo=nuget&style=for-the-badge&#41;]&#40;https://www.nuget.org/packages?q=NeerCore&#41;)

[![GitHub license](https://img.shields.io/github/license/jurilents/NeerCore?color=512bd4&logo=github&style=flat-square)](https://github.com/jurilents/NeerCore/blob/master/LICENSE)
[![GitHub Actions](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fatrox%2Fsync-dotenv%2Fbadge&label=publish&style=flat-square)](https://actions-badge.atrox.dev/jurilents/NeerCore/badge)

[//]: # ([![NuGet Pre-Release]&#40;https://img.shields.io/endpoint?color=2AABEE&label=telegram&style=flat-square&url=https%3A%2F%2Frunkit.io%2Fdamiankrawczyk%2Ftelegram-badge%2Fbranches%2Fmaster%3Furl%3Dhttps%3A%2F%2Ft.me%2Fdotnetme&#41;]&#40;https://t.me/dotnetme&#41;)

</div>

<div align="center">
    <img src="favicon.png" alt="logo" height="200"/>
</div>

<br />

> **Write .NET apps of any complexity in the same style!**

## About

__Heyo bananas!__ 😎🍌🍌

> NeerCore is a set of universal tools that allow you to write a .NET app as before, but without boilerplate stuff :)

NeerCore is a boilerplate library for simplified writing of web applications based on ASP.NET 6. It integrates a popular
set of core libraries that are required for developing full-fledged apps, but they don't come out of the box in ASP.NET.
NeerCore can be used as shown in the examples in the official repository, or separately in parts that you need. You can
also use part of the code from the repository if you do not need to include "one more nuget package" or if your project
is not support the version of .NET on which NeerCore was created.

Thank you for using my developments, I hope they make your life easier! ;)

-----------------------------

## Getting started

You can use one of provided templates for your web or console application.

### [Console app with DI](https://github.com/jurilents/NeerCore-Examples-ConsoleDependencyInjection)

A simple example of a console application using Dependency Injection.

### [Simple API](https://github.com/jurilents/NeerCore-Examples-SimpleApi)

> I recommend it as your first NeerCore project template 😏.

All what you need in single project. Minimal template for simple projects.

### [Services-based API](https://github.com/jurilents/NeerCore-Examples-ServiceBasedApi)

Just a clean API template and nothing more :)

### [CQRS-based API](https://github.com/jurilents/NeerCore-Examples-MediatorBasedApi)

Clean CQRS Architecture template. All what your need to get started your new  _💡idea💡_!

### [CQRS-based API with more features](https://github.com/jurilents/NeerCore-Examples-CompletedApi)

Clean CQRS Architecture template with more features. Provided features: EF Identity, Localization, JWT Auth.

-----------------------------

## Features navigation

### [NeerCore](https://www.nuget.org/packages/NeerCore)

- Core used by another `NeerCore.*` libs
- [Exceptions that represents themost common HTTP errors](https://github.com/jurilents/NeerCore/wiki/HTTP-Exceptions)
- [Extensions for `String`, `Enum`, `IEnumerable` and `ClaimsPrincipal`](https://github.com/jurilents/NeerCore/wiki/Extension-Methods)
- [New `System.Text.Json` policies and conventions](https://github.com/jurilents/NeerCore/wiki/JSON-Conventions-and-Policies)

### [NeerCore.Api](https://www.nuget.org/packages/NeerCore.Api)

- [Base Controllers](https://github.com/jurilents/NeerCore/wiki/Web-API-Controllers)
- [Kebab-case Routes by Default](https://github.com/jurilents/NeerCore/wiki/Web-API-Kebab-Case-Routes)
- [API Versioning](https://github.com/jurilents/NeerCore/wiki/Web-API-Versioning)
- [Smart injection of Middlewares in Factory style](https://github.com/jurilents/NeerCore/wiki/Web-API-Factory-Middlewares)
- [Exception Handler Middleware](https://github.com/jurilents/NeerCore/wiki/Web-API-Exception-Handler-Middleware)
- [`Swagger` + `ReDoc`](https://github.com/jurilents/NeerCore/wiki/Web-API-Swagger-and-ReDoc)
- JsonPatchDocument Swagger integration
- Elegant Navigation Headers

### [NeerCore.Application.MediatR](https://www.nuget.org/packages/NeerCore.Application.MediatR)

- [Smart injection of `MediatR` + `FluentValidation`]
- [Base Controller with Mediator](https://github.com/jurilents/NeerCore/wiki/Web-API-Controllers)
- [Mediator interfaces]

### [NeerCore.Data](https://www.nuget.org/packages/NeerCore.Data)

- [Localized String Type]

### [NeerCore.Data.Abstractions](https://www.nuget.org/packages/NeerCore.Data.Abstractions)

- Shared DB abstractions (used in one bundle with NeerCore.Data.* libs)
- [Entity Abstractions]

### [NeerCore.Data.EntityFramework](https://www.nuget.org/packages/NeerCore.Data.EntityFramework)

- [Smart Data Seeding]
- [`DbContext` Factory]
- [The Most Common `IQueryable` Extensions]

### [NeerCore.DependencyInjection](https://www.nuget.org/packages/NeerCore.DependencyInjection)

- [Attribute-based Dependency Injection ToolKit](https://github.com/jurilents/NeerCore/wiki/Smart-Dependency-Injection)
- [Configure options with `IConfigureOptions<>`]

### [NeerCore.Logging.NLog](https://www.nuget.org/packages/NeerCore.Logging.NLog)

- [Preconfigured from the box file+console logger]

### [NeerCore.Mapping.Mapster](https://www.nuget.org/packages/NeerCore.Mapping.Mapster)

- [Smart Mapster Registration]
- [Mapster Extensions]

-----------------------------

## Contributors

| `@jurilents` | [telegram](https://t.me/nocitats) | author, greatest banana 🍌 |
|--------------|-----------------------------------|----------------------------|
