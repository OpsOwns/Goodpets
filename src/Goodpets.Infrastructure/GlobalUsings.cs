// Global using directives

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Goodpets.Domain.Abstractions;
global using Goodpets.Domain.Users;
global using Goodpets.Domain.Users.Repositories;
global using Goodpets.Domain.Users.ValueObjects;
global using Goodpets.Infrastructure.Database;
global using Goodpets.Infrastructure.Security;
global using Goodpets.Infrastructure.Security.Abstractions;
global using Goodpets.Infrastructure.Security.Auth;
global using Goodpets.Infrastructure.Security.Options;
global using Goodpets.Infrastructure.SeedWork;
global using Goodpets.Shared.Abstractions;
global using Goodpets.Shared.Database;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;