FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Goodpets.API/Goodpets.API.csproj", "Goodpets.API/"]
COPY ["Goodpets.Application/Goodpets.Application.csproj", "Goodpets.Application/"]
COPY ["Goodpets.Infrastructure/Goodpets.Infrastructure.csproj", "Goodpets.Infrastructure/"]
COPY ["Goodpets.Domain/Goodpets.Domain.csproj", "Goodpets.Domain/"]
COPY ["Goodpets.Shared/Goodpets.Shared.csproj", "Goodpets.Shared/"]
RUN dotnet restore "Goodpets.API/Goodpets.API.csproj"
COPY . .
WORKDIR "/src/Goodpets.API"
RUN dotnet build "Goodpets.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goodpets.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goodpets.API.dll"]
