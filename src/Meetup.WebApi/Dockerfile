#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Meetup.WebApi/Meetup.WebApi.csproj", "src/Meetup.WebApi/"]
COPY ["src/Meetup.Core.Application/Meetup.Core.Application.csproj", "src/Meetup.Core.Application/"]
COPY ["src/Meetup.Core.Domain/Meetup.Core.Domain.csproj", "src/Meetup.Core.Domain/"]
COPY ["src/Meetup.Infrastructure/Meetup.Infrastructure.csproj", "src/Meetup.Infrastructure/"]
RUN dotnet restore "./src/Meetup.WebApi/Meetup.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Meetup.WebApi"
RUN dotnet build "./Meetup.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Meetup.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meetup.WebApi.dll"]