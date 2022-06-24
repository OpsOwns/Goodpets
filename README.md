# Sample Backend part of system 'goodpets'

# CI

[![Goodpets](https://github.com/OpsOwns/Goodpets/actions/workflows/main.yml/badge.svg?branch=master)](https://github.com/OpsOwns/Goodpets/actions/workflows/main.yml)

# Introducation (purpose of the system)

The purpose of the system is help vet manage his clinic to work more efficiently and take less energy to "fill papers".
System should guarantee faster fill all stuff what is needed to register owner with his pet. The owner can after visit
check status of his pet, what pills or other treatment should take.


# Technologies & Practises

* .NET 6 ⭐
* DDD (Domain Driven Design) approach ⭐
* Clean architecture ⭐
* CQRS ⭐
* Unit Tests ⭐

# Project structure

* Domain (entities, value objects, aggregates, repositories interfaces, exceptions etc.)
* Infrastructure (Database operations, authentication etc.)
* Application (Domain logic implementation)
* Api (Presentation layer)

# Project assumptions

### Domain

* Entities was created with FactoryMethod
* ValueObjects was created with FactoryMethod called 'Create'
* To avoid throwing to much exceptions, it was decided to return Result of method.
* Stored interfaces of repositories

### Infrastructure

* Implementation repositories body from domain interfaces.
* Implementation Query Handlers because there is no reason to store it in application layer, becouse they don't do any buisness logic
* Implementation email service for sending email to owners.
* Implementation all stuff regarding security generate tokens, auth, etc.
* Every Command handler is wrapped into Unit Of Work by used decorators.

### Application

* Implementation commands where is done business logic with database based on repositories interfaces
* Implementaion Dtos and queries.
* All interfaces which one implemented in infrastructure layer, allow to invoke methods from Infrastructure.

### API
* Handle all controllers
* Validations (used FluentValidation)
* Requests
* Custom error responses with result methods

# Built With

* [Entity Framework](https://github.com/dotnet/efcore)
* [Humanizer](https://github.com/Humanizr/Humanizer)
* [FluentValidation](https://github.com/FluentValidation/FluentValidation)
* [MailKit](https://github.com/jstedfast/MailKit)
* [Docker Compose](https://docs.docker.com/compose/)
* [Azure Key Vault](https://github.com/Azure/AzureKeyVault)
* [Xunit](https://github.com/xunit/xunit)
* [Shoudly](https://github.com/shouldly/shouldly)
* [NSubstitute](https://github.com/nsubstitute/NSubstitute)
* [FluentResults](https://github.com/altmann/FluentResults)
* [NodaTime](https://github.com/nodatime/nodatime)

# Storage
* MS sql server

# How to start the solution?

Type the following command:

```
docker-compose up -d
```

# License

This project is licensed under the [MIT License](LICENSE.md).
