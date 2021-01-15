FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS vs-workaround

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
RUN apt-get update && apt-get install -y unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /usr/local/bin/vsdbg/vsdbg
COPY ["okteto-dotnet-poc.sln", "."]
COPY ["okteto-dotnet-poc.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build \
    --configuration Release \
    --output /build

FROM build as publish
RUN dotnet publish \
    --configuration Release \
    --output /publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
WORKDIR /app
COPY --from=publish /publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "okteto-dotnet-poc.dll"]