FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Notification.Consumer/Notification.Consumer.csproj", "src/Notification.Consumer/"]
RUN dotnet restore "src/Notification.Consumer/Notification.Consumer.csproj"
COPY . .
WORKDIR "/src/src/Notification.Consumer"
RUN dotnet build "Notification.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.Consumer.dll"]
