# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 8080

RUN mkdir -p /app/data

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tarifa/Tarifa.Api/Tarifa.Api.csproj", "Tarifa/Tarifa.Api/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Tarifa/Tarifa.Infrastructure/Tarifa.Infrastructure.csproj", "Tarifa/Tarifa.Infrastructure/"]
COPY ["Core.Infrastructure/Core.Infrastructure.csproj", "Core.Infrastructure/"]
COPY ["Tarifa/Tarifa.Application/Tarifa.Application.csproj", "Tarifa/Tarifa.Application/"]
COPY ["Tarifa/Tarifa.Domain/Tarifa.Domain.csproj", "Tarifa/Tarifa.Domain/"]
RUN dotnet restore "./Tarifa/Tarifa.Api/Tarifa.Api.csproj"
COPY . .
WORKDIR "/src/Tarifa/Tarifa.Api"
RUN dotnet build "./Tarifa.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tarifa.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tarifa.Api.dll"]