FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/CreatedItemsConsumer/CreatedItemsConsumer.csproj", "Services/CreatedItemsConsumer/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./Services/CreatedItemsConsumer/./CreatedItemsConsumer.csproj"
COPY /src .
WORKDIR "/src/Services/CreatedItemsConsumer"
RUN dotnet build "./CreatedItemsConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CreatedItemsConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreatedItemsConsumer.dll"]