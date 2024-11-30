#!/bin/bash

# Check if gum is installed
if ! command -v gum &> /dev/null; then
    echo "Error: gum is not installed. Please install it first."
    echo "To install gum, visit: https://github.com/charmbracelet/gum#installation"
    echo "If you are on a Mac: 'brew install gum'"
    echo "If you are on Linux: 'sudo apt-get install gum' or 'sudo pacman -S gum'"
    exit 1
fi

# Function to run commands in a new tmux pane
run_command() {
    tmux split-window -h
    tmux send-keys "$1" C-m
}

rebase(){

	rm -rf Migrations

	rm ProjectMVC.db

	dotnet ef migrations add Initial

	dotnet ef database update
}

# Start a new tmux session
tmux new-session -d -s something_nice

# Use gum to choose which components to run
CHOICE=$(gum choose --header "Do you want to run the frontend or backend?" --item.foreground 250 "Frontend" "Rebuild")

case $CHOICE in
    "Frontend")
        tmux send-keys "npx tailwindcss -i ./wwwroot/css/site.css -o ./wwwroot/css/output.css --watch" C-m
        run_command "dotnet watch"
        ;;
	"Rebuild")
		rebase
        tmux send-keys "npx tailwindcss -i ./wwwroot/css/site.css -o ./wwwroot/css/output.css --watch" C-m
        run_command "dotnet watch --no-hot-reload"
		;;
    *)
        echo "Invalid choice. Exiting."
        tmux kill-session -t webapp_social
        exit 1
        ;;
esac

# Attach to the tmux session
tmux attach-session -t something_nice
