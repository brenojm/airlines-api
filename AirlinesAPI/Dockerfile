# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) restore em camada separada
COPY ["AirlinesAPI.csproj", "./"]
RUN dotnet restore

# 2) copie o restante e publique
COPY . .
RUN dotnet publish -c Release -o /app

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AirlinesAPI.dll"]
