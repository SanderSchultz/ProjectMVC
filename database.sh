#!/bin/bash

rm -rf Migrations

rm ProjectMVC.db

dotnet ef migrations add Initial

dotnet ef database update
