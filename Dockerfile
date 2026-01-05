# Stage 1: Build - Compila toda a solução
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução e os arquivos de projeto (.csproj) primeiro
# Isso permite que o Docker cache a camada de restauração de dependências
COPY BankMore.sln .
COPY Core/*.csproj Core/
COPY Core.Infrastructure/*.csproj Core.Infrastructure/
COPY ContaCorrente/ContaCorrente.Api/*.csproj ContaCorrente/ContaCorrente.Api/
COPY ContaCorrente/ContaCorrente.Application/*.csproj ContaCorrente/ContaCorrente.Application/
COPY ContaCorrente/ContaCorrente.Domain/*.csproj ContaCorrente/ContaCorrente.Domain/
COPY ContaCorrente/ContaCorrente.Infrastructure/*.csproj ContaCorrente/ContaCorrente.Infrastructure/
COPY Transferencia/Transferencia.Api/*.csproj Transferencia/Transferencia.Api/
COPY Transferencia/Transferencia.Application/*.csproj Transferencia/Transferencia.Application/
COPY Transferencia/Transferencia.Domain/*.csproj Transferencia/Transferencia.Domain/
COPY Transferencia/Transferencia.Infrastructure/*.csproj Transferencia/Transferencia.Infrastructure/

# Restaura as dependências para toda a solução
RUN dotnet restore BankMore.sln

# Copia o restante do código-fonte
COPY . .

# Stage 2: Publish ContaCorrente.Api
FROM build AS publish-contacorrente
WORKDIR /src/ContaCorrente/ContaCorrente.Api
RUN dotnet publish "ContaCorrente.Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Stage 3: Runtime ContaCorrente.Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-contacorrente
WORKDIR /app
COPY --from=publish-contacorrente /app/publish .
# Cria o diretório para o banco de dados SQLite persistente
RUN mkdir -p /app/data
ENTRYPOINT ["dotnet", "ContaCorrente.Api.dll"]

# Stage 4: Publish Transferencia.Api
FROM build AS publish-transferencia
WORKDIR /src/Transferencia/Transferencia.Api
RUN dotnet publish "Transferencia.Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Stage 5: Runtime Transferencia.Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-transferencia
WORKDIR /app
COPY --from=publish-transferencia /app/publish .
# Cria o diretório para o banco de dados SQLite persistente
RUN mkdir -p /app/data
ENTRYPOINT ["dotnet", "Transferencia.Api.dll"]