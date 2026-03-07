# Etapa base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
ENV ASPNETCORE_URLS=http://+:80
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CompraProgramada.Api.dll"]