FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
Workdir /src

COPY SistemaCompras.sln .
COPY SistemaCompras.Api/SistemaCompras.Api.csproj							./SistemaCompras.Api/
COPY SistemaCompras.Application/SistemaCompras.Application.csproj			./SistemaCompras.Application/
COPY SistemaCompras.Domain/SistemaCompras.Domain.csproj						./SistemaCompras.Domain/
COPY SistemasCompras.Domain.Core/SistemasCompras.Domain.Core.csproj			./SistemasCompras.Domain.Core/
COPY SistemaCompras.Infra.Data/SistemaCompras.Infra.Data.csproj				./SistemaCompras.Infra.Data/

RUN dotnet restore

COPY . .

RUN dotnet publish SistemaCompra.API/SistemaCompra.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SistemaCompra.API.dll"]