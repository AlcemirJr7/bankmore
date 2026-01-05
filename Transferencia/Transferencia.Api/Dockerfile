# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

RUN mkdir -p /app/data

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Transferencia/Transferencia.Api/Transferencia.Api.csproj", "Transferencia/Transferencia.Api/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Transferencia/Transferencia.Infrastructure/Transferencia.Infrastructure.csproj", "Transferencia/Transferencia.Infrastructure/"]
COPY ["Core.Infrastructure/Core.Infrastructure.csproj", "Core.Infrastructure/"]
COPY ["Transferencia/Transferencia.Application/Transferencia.Application.csproj", "Transferencia/Transferencia.Application/"]
COPY ["Transferencia/Transferencia.Domain/Transferencia.Domain.csproj", "Transferencia/Transferencia.Domain/"]
RUN dotnet restore "./Transferencia/Transferencia.Api/Transferencia.Api.csproj"
COPY . .
WORKDIR "/src/Transferencia/Transferencia.Api"
RUN dotnet build "./Transferencia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Transferencia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transferencia.Api.dll"]