FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["speed-of-stuff/speed-of-stuff.csproj", "speed-of-stuff/"]
RUN dotnet restore "speed-of-stuff/speed-of-stuff.csproj"
COPY . .
WORKDIR "/src/speed-of-stuff"
RUN dotnet build "speed-of-stuff.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "speed-of-stuff.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "speed-of-stuff.dll"]