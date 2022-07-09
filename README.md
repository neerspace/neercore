<h1 align="center">NeerCore ↗⭐↗</h1>

--------------------------------

<div align="center">

[![NuGet Release](https://img.shields.io/nuget/v/NeerCore?label=Actual&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages?q=NeerCore)
[![NuGet Pre-Release](https://img.shields.io/nuget/vpre/NeerCore?label=Latest&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/NeerCore)
[![NuGet Pre-Release](https://img.shields.io/nuget/dt/NeerCore.DependencyInjection?color=512bd4&style=for-the-badge)](https://www.nuget.org/packages/NeerCore)

[![GitHub license](https://img.shields.io/github/license/jurilents/NeerCore?color=512bd4&logo=github&style=flat-square)](https://github.com/jurilents/NeerCore/blob/master/LICENSE)
[![GitHub Actions](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fatrox%2Fsync-dotenv%2Fbadge&label=publish&style=flat-square)](https://actions-badge.atrox.dev/jurilents/NeerCore/badge)
[![NuGet Pre-Release](https://img.shields.io/endpoint?color=2AABEE&label=telegram&style=flat-square&url=https%3A%2F%2Frunkit.io%2Fdamiankrawczyk%2Ftelegram-badge%2Fbranches%2Fmaster%3Furl%3Dhttps%3A%2F%2Ft.me%2Fdotnetme)](https://t.me/dotnetme)

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

## Provided features

### [NeerCore](https://www.nuget.org/packages/NeerCore)

- Core used by another `NeerCore.*` libs
- Exceptions that represents most common HTTP errors
- Extensions for `String`, `Enum`, `IEnumerable` and `ClaimsPrincipal`
- New JSON policies and conventions

### [NeerCore.DependencyInjection](https://www.nuget.org/packages/NeerCore.DependencyInjection)

- Attribute-based Dependency Injection ToolKit
- Extensions for `Assembly` and `Type`
- Some core reflection utilities

### [NeerCore.Data.Abstractions](https://www.nuget.org/packages/NeerCore.Data.Abstractions)

- Shared DB abstractions (used in one bundle with NeerCore.Data.* libs)

### [NeerCore.Data](https://www.nuget.org/packages/NeerCore.Data)

- Shared DB data classes with custom primitives
- EntityFrameworkCore localized string type

### [NeerCore.Data.EntityFramework](https://www.nuget.org/packages/NeerCore.Data.EntityFramework)

- Simpler way to use EntityFrameworkCore
- `IDatabaseContext` abstraction to not use `DbContext` class directly
- The much better `DbContextFactory`
- Extensions for `ModelBuilder` and `IQueryable`

### [NeerCore.Api](https://www.nuget.org/packages/NeerCore.Api)

- ASP.NET Core WEb API ToolKit
- `WebApplicationBuilder` extensions to integrate entry API initialization with a single method `AddNeerApi()`
- Swagger and ReDoc documentation out of the box
- Native Swagger support for `JsonPatchDocument`
- _kebab-case-route-naming-by-default_
- Custom exception handling middleware
- NLog integration
- Elegant navigation headers
- `IFactoryMiddleware` injection by default
- Extensions for `HttpContext`, `HttpResponse` and `JsonPatchDocument`

### [NeerCore.Application.MediatR](https://www.nuget.org/packages/NeerCore.Application.MediatR)

- CQRS pattern integration
- MediatrR + FluentValidation

### [NeerCore.Mapping.Mapster](https://www.nuget.org/packages/NeerCore.Mapping.Mapster)

- Mapster `IRegister` DI integration
- Extensions for `IEnumerable`

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

## Contributors

| `@jurilents` | [telegram](https://t.me/nocitats) | author, greatest banana 🍌 |
|--------------|-----------------------------------|----------------------------|
