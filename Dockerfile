# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app
COPY . .

EXPOSE 5000

RUN dotnet publish -c Release -o publish

# Stage 2: Create the runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
WORKDIR /app
LABEL maintainer="Ivan Evchatov"
ENV ENVIRONMENT=stage

COPY --from=build /app/publish .

USER nobody
EXPOSE 5000

ENTRYPOINT ["dotnet", "food_delivery.dll"]
