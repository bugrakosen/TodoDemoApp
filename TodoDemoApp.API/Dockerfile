#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5003
EXPOSE 5003/tcp

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TodoDemoApp.API/TodoDemoApp.API.csproj", "TodoDemoApp.API/"]
RUN dotnet restore "TodoDemoApp.API/TodoDemoApp.API.csproj"
COPY . .
WORKDIR "/src/TodoDemoApp.API"
RUN dotnet build "TodoDemoApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoDemoApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
    
ENTRYPOINT ["dotnet", "TodoDemoApp.API.dll"]