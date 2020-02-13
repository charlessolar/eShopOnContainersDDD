
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/", "/"]
RUN dotnet restore "Endpoints/Elastic/Elastic.csproj"
WORKDIR /src/src/Endpoints/Elastic
RUN dotnet build "Endpoints/Elastic/Elastic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Endpoints/Elastic/Elastic.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elastic.dll"]
