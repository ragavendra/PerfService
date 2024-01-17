FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY PerfRunner.csproj .
RUN dotnet restore
# PerfRunner/*.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 5110
EXPOSE 5000
EXPOSE 5277
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS "http://+:5000"
COPY --from=build-env /app .
#ENTRYPOINT ["dotnet", "PerfRunner.dll", "--environment=Development"]
ENTRYPOINT ["dotnet", "PerfRunner.dll"]
