FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WaterDelivery.UI/WaterDelivery.UI.csproj", "WaterDelivery.UI/"]
RUN dotnet restore "WaterDelivery.UI/WaterDelivery.UI.csproj"
COPY . .
WORKDIR "/src/WaterDelivery.UI"
RUN dotnet build "WaterDelivery.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WaterDelivery.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WaterDelivery.UI.dll"]
