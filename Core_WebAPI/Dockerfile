# ===============================
# Stage 1: Build the application
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish in Release mode
RUN dotnet publish -c Release -o /app/publish


# ===============================
# Stage 2: Runtime image
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose the port Railway uses
EXPOSE 8080

# Tell ASP.NET Core to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Start the application
ENTRYPOINT ["dotnet", "Core_WebAPI.dll"]
