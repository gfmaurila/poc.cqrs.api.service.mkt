# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5074

ENV ASPNETCORE_URLS=http://+:5074
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY mock/poc.mock.sendgrid/poc.mock.sendgrid.csproj mock/poc.mock.sendgrid/

RUN dotnet restore mock/poc.mock.sendgrid/poc.mock.sendgrid.csproj

COPY . .
WORKDIR /src/mock/poc.mock.sendgrid
RUN dotnet build poc.mock.sendgrid.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish poc.mock.sendgrid.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc.mock.sendgrid.dll"]