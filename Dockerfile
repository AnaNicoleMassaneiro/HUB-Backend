FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet restore HubUfpr.NetCore.sln
RUN dotnet build HubUfpr.NetCore.sln --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish HubUfpr.NetCore.sln --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
# Padrão de container ASP.NET
ENTRYPOINT ["dotnet", "HubUfpr.API.dll"]
# Opção utilizada pelo Heroku
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet HubUfpr.API.dll
