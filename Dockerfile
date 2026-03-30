FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SistemaCompras.sln .
COPY SistemaCompra.API/SistemaCompra.API.csproj                 ./SistemaCompra.API/
COPY SistemaCompra.Application/SistemaCompra.Application.csproj ./SistemaCompra.Application/
COPY SistemaCompra.Domain/SistemaCompra.Domain.csproj           ./SistemaCompra.Domain/
COPY SistemaCompra.Domain.Core/SistemaCompra.Domain.Core.csproj ./SistemaCompra.Domain.Core/
COPY SistemaCompra.Infra.Data/SistemaCompra.Infra.Data.csproj   ./SistemaCompra.Infra.Data/

RUN dotnet restore SistemaCompra.API/SistemaCompra.API.csproj


COPY . .

RUN dotnet publish SistemaCompra.API/SistemaCompra.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SistemaCompra.API.dll"]
