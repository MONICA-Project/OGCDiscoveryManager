FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["OGCDiscoveryManager/OGCDiscoveryManager.csproj", "OGCDiscoveryManager/"]
RUN dotnet restore "OGCDiscoveryManager/OGCDiscoveryManager.csproj"
COPY . .
WORKDIR "/src/OGCDiscoveryManager"
RUN dotnet build "OGCDiscoveryManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OGCDiscoveryManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OGCDiscoveryManager.dll"]