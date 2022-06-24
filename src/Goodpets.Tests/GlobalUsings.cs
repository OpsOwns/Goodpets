// Global using directives

global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Bogus;
global using Goodpets.Application.Abstractions;
global using Goodpets.Application.Abstractions.Security;
global using Goodpets.Application.Abstractions.SeedWork;
global using Goodpets.Application.SeedWork;
global using Goodpets.Application.User.Commands;
global using Goodpets.Application.User.Commands.Handlers;
global using Goodpets.Application.User.DTO;
global using Goodpets.Domain.Entities;
global using Goodpets.Domain.Repositories;
global using Goodpets.Domain.SeedWork;
global using Goodpets.Domain.Types;
global using Goodpets.Domain.ValueObjects;
global using Goodpets.Tests.FakeData;
global using NodaTime;
global using NSubstitute;
global using Shouldly;
global using Xunit;