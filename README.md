# Sample Backend part of system 'goodpets'

[![Goodpets](https://github.com/OpsOwns/Goodpets/actions/workflows/main.yml/badge.svg?branch=master)](https://github.com/OpsOwns/Goodpets/actions/workflows/main.yml)

## Introducation (purpose of the system)

The purpose of the system is help vet manage his clinic to work more efficiently and take less energy to "fill papers".
System should guarantee faster fill all stuff what is needed to register owner with his pet. The owner can after visit
check status of his pet, what pills or other treatment should take.


### Technologies & Practises

* .NET 6 ⭐
* DDD (Domain Driven Design) approach ⭐
* Clean architecture ⭐
* CQRS ⭐
* Unit Tests ⭐

### Project structure

* Domain (entities, value objects, aggregates, repositories interfaces, exceptions etc.)
* Infrastructure (Database operations, authentication etc.)
* Application (Domain logic implementation)
* Api (Presentation layer)

### Built With

* [Entity Framework](https://github.com/dotnet/efcore)
* [Humanizer](https://github.com/Humanizr/Humanizer)
* [FluentValidation](https://github.com/FluentValidation/FluentValidation)
* [MailKit](https://github.com/jstedfast/MailKit)
* [Docker Compose](https://docs.docker.com/compose/)
* [Azure Key Vault](https://github.com/Azure/AzureKeyVault)
* [Xunit](https://github.com/xunit/xunit)
* [Moq](https://github.com/moq/moq)

### Storage
* MS sql server

### How to start the solution?

Type the following command:

```
docker-compose up -d
```

### License

This project is licensed under the [MIT License](LICENSE.md).
