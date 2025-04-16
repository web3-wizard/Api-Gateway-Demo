FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 5321


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln .
COPY ProductApi/ProductApi.csproj ./ProductApi/
RUN dotnet restore
COPY . .
WORKDIR /src/ProductApi/
RUN dotnet build ProductApi.csproj -c $BUILD_CONFIGURATION -o /app/build --no-restore


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ProductApi.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:5321
ENTRYPOINT ["dotnet", "ProductApi.dll"]