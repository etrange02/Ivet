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
RUN apk add --no-cache dotnet9-runtime && \
    adduser -D -h /app ivet
COPY --from=build-env /app/out/ /app
COPY docker-entrypoint.sh /app/
RUN chmod +x /app/docker-entrypoint.sh
USER ivet
WORKDIR /app
ENV input=/app
ENV ip=localhost
ENV port=8182
ENV ssl=false
ENV timeout=
ENTRYPOINT ["/app/docker-entrypoint.sh"]
