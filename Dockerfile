FROM alpine:latest as build-env
RUN apk update
RUN apk add dotnet9-sdk dotnet8-sdk
WORKDIR /app
COPY . ./
RUN dotnet restore
# COPY \bin\Release\net6.0\publish/* /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM alpine:latest
RUN apk update
RUN apk add dotnet9-runtime
COPY --from=build-env /app/out/ /app
#COPY --from=build-env /app/Migrations/ /app/Migrations/
ENV input=/app
#/app/Migrations
ENV ip=localhost
ENV post=8182
ENTRYPOINT ["sh", "-c", "dotnet /app/Ivet.dll upgrade --input $input --ip $ip --port $port"]

# docker run -v D:\Workspace\Projects\Ivet\Migrations2:/app/Migrations -e ip=192.168.1.11 ivet
