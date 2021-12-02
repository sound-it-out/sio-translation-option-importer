#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SIO.TranslationOption.Importer/SIO.TranslationOption.Importer.csproj", "src/SIO.TranslationOption.Importer/"]
COPY ["src/SIO.Migrations/SIO.Migrations.csproj", "src/SIO.Migrations/"]
COPY ["src/SIO.Domain/SIO.Domain.csproj", "src/SIO.Domain/"]
RUN dotnet restore "src/SIO.TranslationOption.Importer/SIO.TranslationOption.Importer.csproj"
COPY . .
WORKDIR "/src/src/SIO.TranslationOption.Importer"
RUN dotnet build "SIO.TranslationOption.Importer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SIO.TranslationOption.Importer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIO.TranslationOption.Importer.dll"]