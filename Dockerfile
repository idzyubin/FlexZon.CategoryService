ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.sln .

COPY ./src/FlexZon.CategoryService.Api/*.csproj ./src/FlexZon.CategoryService.Api/
COPY ./src/FlexZon.CategoryService.Application/*.csproj ./src/FlexZon.CategoryService.Application/
COPY ./src/FlexZon.CategoryService.Contract/*.csproj ./src/FlexZon.CategoryService.Contract/

RUN dotnet restore

COPY ./src/FlexZon.CategoryService.Api/. ./src/FlexZon.CategoryService.Api/
COPY ./src/FlexZon.CategoryService.Application/. ./src/FlexZon.CategoryService.Application/
COPY ./src/FlexZon.CategoryService.Contract/. ./src/FlexZon.CategoryService.Contract/

RUN dotnet build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish \
    -c $BUILD_CONFIGURATION \
    -o /app/publish -p:PublishTrimmed=true  \
    --runtime linux-x64  \
    --self-contained true

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["/app/FlexZon.CategoryService.Api"]