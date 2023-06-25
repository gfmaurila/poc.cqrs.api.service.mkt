# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5072

ENV ASPNETCORE_URLS=http://+:5072
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY service/Mkt.Api/Mkt.Api.csproj service/Mkt.Api/
COPY core/Mkt.Infra/Mkt.Infra.csproj core/Mkt.Infra/
COPY core/Mkt.App/Mkt.App.csproj core/Mkt.App/
COPY core/Mkt.Core/Mkt.Core.csproj core/Mkt.Core/

RUN dotnet restore service/Mkt.Api/Mkt.Api.csproj

COPY . .
WORKDIR /src/service/Mkt.Api
RUN dotnet build Mkt.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Mkt.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mkt.Api.dll"]