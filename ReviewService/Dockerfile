FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 5421


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln .
COPY ReviewApi/ReviewApi.csproj ./ReviewApi/
RUN dotnet restore
COPY . .
WORKDIR /src/ReviewApi/
RUN dotnet build ReviewApi.csproj -c $BUILD_CONFIGURATION -o /app/build --no-restore


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ReviewApi.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://0.0.0.0:5421
ENTRYPOINT ["dotnet", "ReviewApi.dll"]