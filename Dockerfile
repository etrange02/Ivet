# Stage 1: Base environment
FROM alpine:3.22.1 AS base
RUN apk update
	
FROM base as build-env
RUN apk add --no-cache dotnet9-sdk
WORKDIR /app
COPY . ./
RUN dotnet restore && \
	dotnet publish -c Release -o out

# Build runtime image
FROM base as final
RUN apk add --no-cache dotnet9-runtime
COPY --from=build-env /app/out/ /app
#COPY --from=build-env /app/Migrations/ /app/Migrations/
ENV input=/app
#/app/Migrations
ENV ip=localhost
ENV post=8182
ENTRYPOINT ["sh", "-c", "dotnet /app/Ivet.dll upgrade --input $input --ip $ip --port $port"]

# docker run -v D:\Workspace\Projects\Ivet\Migrations2:/app/Migrations -e ip=192.168.1.11 ivet
