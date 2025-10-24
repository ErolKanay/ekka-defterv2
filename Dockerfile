FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApplication1/WebApplication1/WebApplication1.fsproj", "WebApplication1/WebApplication1/"]
RUN dotnet restore "WebApplication1/WebApplication1/WebApplication1.fsproj"
COPY . .
WORKDIR "/src/WebApplication1/WebApplication1"
RUN dotnet build "WebApplication1.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication1.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
