FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/GameWatch/GameWatch.csproj", "src/GameWatch/"]
RUN dotnet restore "GameWatch/GameWatch.csproj"
COPY . .
WORKDIR "/src/GameWatch"
RUN dotnet build "GameWatch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameWatch.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameWatch.dll"]
