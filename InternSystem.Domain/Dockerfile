FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InternSystem.Domain/InternSystem.Domain.csproj", "InternSystem.Domain/"]
RUN dotnet restore "InternSystem.Domain/InternSystem.Domain.csproj"
COPY . .
WORKDIR "/src/InternSystem.Domain"
RUN dotnet build "InternSystem.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InternSystem.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternSystem.Domain.dll"]
