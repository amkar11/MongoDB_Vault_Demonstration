# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MongoDB_Vault_Demonstration/MongoDB_Vault_Demonstration.csproj", "MongoDB_Vault_Demonstration/"]
RUN dotnet restore "./MongoDB_Vault_Demonstration/MongoDB_Vault_Demonstration.csproj"
COPY . .
WORKDIR "/src/MongoDB_Vault_Demonstration"
RUN dotnet build "./MongoDB_Vault_Demonstration.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MongoDB_Vault_Demonstration.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./vault-ca-root-pki-int.pem /tmp/vault-ca-root-pki-int.pem
USER root
RUN apt-get update \
 && apt-get install -y --no-install-recommends ca-certificates \
 && cp /tmp/vault-ca-root-pki-int.pem /usr/local/share/ca-certificates/vault-ca.crt \
 && update-ca-certificates \
 && rm -rf /var/lib/apt/lists/* /tmp/vault-ca-root-pki-int.pem
ENTRYPOINT ["dotnet", "MongoDB_Vault_Demonstration.dll"]