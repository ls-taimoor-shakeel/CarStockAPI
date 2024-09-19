# Use the official .NET 8.0 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file and restore any dependencies (via NuGet)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Build the project
RUN dotnet build -c Release -o out

# Publish the project
RUN dotnet publish -c Release -o out

# Use the official .NET 8.0 runtime image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build environment
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 80

# Define the entry point for the application
ENTRYPOINT ["dotnet", "CarStockAPI.dll"]
