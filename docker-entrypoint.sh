#!/bin/sh
set -eu
exec dotnet /app/Ivet.dll upgrade --input "$input" --ip "$ip" --port "$port"
