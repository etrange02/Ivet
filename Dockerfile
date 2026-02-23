# Stage 1: Base environment
FROM alpine:3.22.1 AS base

FROM base as build-env
RUN apk add --no-cache dotnet9-sdk
WORKDIR /app
COPY . ./
RUN dotnet restore && \
	dotnet publish Ivet/Ivet.csproj -c Release -o out

# Build runtime image
FROM base as final
RUN apk add --no-cache dotnet9-runtime
COPY --from=build-env /app/out/ /app
ENV input=/app
ENV ip=localhost
ENV port=8182
ENTRYPOINT ["sh", "-c", "dotnet /app/Ivet.dll upgrade --input $input --ip $ip --port $port"]
