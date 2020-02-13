
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/", "/"]
RUN dotnet restore "Endpoints/Domain/Domain.csproj"
WORKDIR /src/src/Endpoints/Domain
RUN dotnet build "Endpoints/Domain/Domain.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Endpoints/Domain/Domain.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Domain.dll"]
