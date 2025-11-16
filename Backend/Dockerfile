    FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
    
    WORKDIR /src
    
    COPY *.sln .
    
    COPY src/MinimalJira.Host/*.csproj ./src/MinimalJira.Host/
    COPY src/MinimalJira.Application/*.csproj ./src/MinimalJira.Application/
    COPY src/MinimalJira.Infrastructure/*.csproj ./src/MinimalJira.Infrastructure/
    COPY src/MinimalJira.Domain/*.csproj ./src/MinimalJira.Domain/
    COPY src/MinimalJira.Persistence/*.csproj ./src/MinimalJira.Persistence/
    COPY src/MinimalJira.Tests/*.csproj ./src/MinimalJira.Tests/
    
    RUN dotnet restore
    
    COPY . .
    
    RUN dotnet build .
    
    RUN dotnet publish src/MinimalJira.Host/MinimalJira.Host.csproj -c Release -o /app
    
    FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
    
    WORKDIR /app
    
    COPY --from=build /app .
    
    EXPOSE 5226
    
    ENTRYPOINT ["dotnet", "MinimalJira.Host.dll"]