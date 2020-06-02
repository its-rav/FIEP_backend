#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FIEP_API/FIEP_API.csproj", "FIEP_API/"]
COPY ["FIEP_API/keys/*", "FIEP_API/keys/*"]
RUN dotnet restore "FIEP_API/FIEP_API.csproj"
COPY . .
WORKDIR "/src/FIEP_API"
RUN dotnet build "FIEP_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FIEP_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIEP_API.dll"]