FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InternSystem.Infrastructure/InternSystem.Infrastructure.csproj", "InternSystem.Infrastructure/"]
RUN dotnet restore "InternSystem.Infrastructure/InternSystem.Infrastructure.csproj"
COPY . .
WORKDIR "/src/InternSystem.Infrastructure"
RUN dotnet build "InternSystem.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InternSystem.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternSystem.Infrastructure.dll"]
