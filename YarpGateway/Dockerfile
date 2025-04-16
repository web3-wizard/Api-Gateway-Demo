FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 5325


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln .
COPY ApiGateway/ApiGateway.csproj ./ApiGateway/
RUN dotnet restore
COPY . .
WORKDIR /src/ApiGateway/
RUN dotnet build ApiGateway.csproj -c $BUILD_CONFIGURATION -o /app/build --no-restore


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ApiGateway.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:5325
ENTRYPOINT ["dotnet", "ApiGateway.dll"]