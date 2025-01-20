# Base image for the .NET app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the entire solution directory (all projects: WebAPI, Application, Domain, Infrastructure)
COPY ./PurchaseOrder.WebAPI /src/PurchaseOrder.WebAPI
COPY ./Application /src/Application
COPY ./Domain /src/Domain
COPY ./Infrastructure /src/Infrastructure
COPY ./Common /src/Common

# Restore dependencies for all projects
RUN dotnet restore "PurchaseOrder.WebAPI/PurchaseOrder.WebAPI.csproj"

# Build the Web API
RUN dotnet build "PurchaseOrder.WebAPI/PurchaseOrder.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the Web API
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PurchaseOrder.WebAPI/PurchaseOrder.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Publish SQL project (.sqlproj)
FROM build AS sqlpublish
COPY ["PO/PO.sqlproj", "PO/"]
RUN dotnet build "PO/PO.sqlproj" -c $BUILD_CONFIGURATION -o /app/sqlbuild
RUN dotnet publish "PO/PO.sqlproj" -c $BUILD_CONFIGURATION -o /app/sqlpublish

# Final stage: combine everything
FROM base AS final
WORKDIR /app

# Copy the published .NET Web API
COPY --from=publish /app/publish .

# Optionally copy the published SQL project if needed
COPY --from=sqlpublish /app/sqlpublish .

# Set the entry point for your .NET Web API
ENTRYPOINT ["dotnet", "PurchaseOrder.WebAPI.dll"]
