#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["News.WebApplication/News.WebApplication.csproj", "News.WebApplication/"]
COPY ["News.Services/News.Services.csproj", "News.Services/"]
COPY ["News.Models/News.Models.csproj", "News.Models/"]
COPY ["News.Utilities/News.Utilities.csproj", "News.Utilities/"]
RUN dotnet restore "News.WebApplication/News.WebApplication.csproj"
COPY . .
WORKDIR "/src/News.WebApplication"
RUN dotnet build "News.WebApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "News.WebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "News.WebApplication.dll"]