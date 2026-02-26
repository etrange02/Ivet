#!/bin/sh
set -eu

set -- --input "$input" --ip "$ip" --port "$port"

if [ "$ssl" = "true" ]; then
    set -- "$@" --ssl
fi

if [ -n "${timeout:-}" ]; then
    set -- "$@" --timeout "$timeout"
fi

exec dotnet /app/Ivet.dll upgrade "$@"
