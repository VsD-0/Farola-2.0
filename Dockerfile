# Этап 1: Сборка
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Копируем файлы решений и проектов для восстановления зависимостей
COPY ["Farola.slnx", "./"]
COPY ["src/Farola.Domain/Farola.Domain.csproj", "src/Farola.Domain/"]
COPY ["src/Farola.Application/Farola.Application.csproj", "src/Farola.Application/"]
COPY ["src/Farola.Infrastructure/Farola.Infrastructure.csproj", "src/Farola.Infrastructure/"]
COPY ["src/Farola.WebApi/Farola.WebApi.csproj", "src/Farola.WebApi/"]

COPY ["src/Farola.Client/Farola.Client.csproj", "src/Farola.Client/"]
COPY ["src/Farola.Shared/Farola.Shared.csproj", "src/Farola.Shared/"]
COPY ["tests/Farola.Domain.Tests/Farola.Domain.Tests.csproj", "tests/Farola.Domain.Tests/"]
COPY ["tests/Farola.Application.Tests/Farola.Application.Tests.csproj", "tests/Farola.Application.Tests/"]
COPY ["tests/Farola.Infrastructure.Tests/Farola.Infrastructure.Tests.csproj", "tests/Farola.Infrastructure.Tests/"]
COPY ["tests/Farola.WebApi.Tests/Farola.WebApi.Tests.csproj", "tests/Farola.WebApi.Tests/"]

RUN dotnet restore "Farola.slnx"

# 2. Копируем весь исходный код и выполняем сборку
COPY . .
RUN dotnet build "Farola.slnx" -c Release -o /app/build

# 3. Публикуем только WebApi проект
FROM build AS publish
RUN dotnet publish "src/Farola.WebApi/Farola.WebApi.csproj" -c Release -o /app/publish

# Этап 2: Финальный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Farola.WebApi.dll"]