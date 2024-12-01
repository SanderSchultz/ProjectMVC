#!/bin/bash

# Ensure the .NET global tools path is in PATH
if [[ ":$PATH:" != *":$HOME/.dotnet/tools:"* ]]; then
    echo "Adding .NET global tools path to PATH..."
    export PATH="$PATH:$HOME/.dotnet/tools"
fi

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET SDK is not installed. Please install .NET 8.0 or higher."
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
if [[ $DOTNET_VERSION != 8.* ]]; then
    echo "Error: .NET SDK 8.0 is required. Current version: $DOTNET_VERSION"
    exit 1
fi

# Check for global or local dotnet-ef installation
DOTNET_EF_GLOBAL=$(command -v dotnet-ef)
DOTNET_EF_LOCAL=".config/dotnet-tools.json"

if [[ -z "$DOTNET_EF_GLOBAL" && ! -f "$DOTNET_EF_LOCAL" ]]; then
    echo "Error: dotnet-ef is not installed. Please install it using:"
    echo "dotnet tool install --global dotnet-ef"
    exit 1
fi

# Restore local tools if dotnet-ef is not globally accessible
if [[ -f "$DOTNET_EF_LOCAL" && -z "$DOTNET_EF_GLOBAL" ]]; then
    echo "Restoring local dotnet tools..."
    dotnet tool restore || { echo "Failed to restore local tools."; exit 1; }
    DOTNET_EF_GLOBAL="dotnet tool run ef"
fi

# Use the proper dotnet-ef command
if [[ -z "$DOTNET_EF_GLOBAL" ]]; then
    echo "Error: Unable to locate dotnet-ef even after restoration. Exiting."
    exit 1
fi

# Migration and application start functions
rebase() {
    echo "Removing old migrations and database..."
    rm -rf Migrations
    rm -f ProjectMVC.db

	cp ./wwwroot/seedimages/* ./wwwroot/images/ || true

    echo "Adding new migration..."
    if ! $DOTNET_EF_GLOBAL migrations add Initial; then
        echo "Failed to add migrations. Ensure 'dotnet ef' is installed and accessible."
        echo "To install globally: dotnet tool install --global dotnet-ef"
        echo "To install locally: dotnet tool install dotnet-ef --local"
        exit 1
    fi

    echo "Updating database..."
    if ! $DOTNET_EF_GLOBAL database update; then
        echo "Failed to update database. Verify your database connection and EF setup."
        exit 1
    fi
}

# Perform migrations and run the application
echo "Running migrations and starting the application..."
rebase
dotnet run
