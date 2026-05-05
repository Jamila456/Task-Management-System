# Dockerfile for Task Management System
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build

# Set working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN nuget restore

# Copy all files
COPY . ./

# Build the application
RUN msbuild /p:Configuration=Release /p:OutputPath=bin/Release

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8

# Set working directory
WORKDIR /app

# Install required fonts and components for WinForms
ADD https://download.visualstudio.microsoft.com/download/pr/01234/abc/WindowsSDK.exe /tmp/sdk.exe
# Note: For WinForms, we need to ensure proper runtime environment

# Copy the built application from build stage
COPY --from=build /app/bin/Release .

# Set environment variable to detect Docker container
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Set the entry point
ENTRYPOINT ["TaskManagementSystem.exe"]