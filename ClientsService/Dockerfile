#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClientsService/ClientsService.csproj", "ClientsService/"]
RUN dotnet restore "ClientsService/ClientsService.csproj"
COPY . .
WORKDIR "/src/ClientsService"
RUN dotnet build "ClientsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientsService.dll"]