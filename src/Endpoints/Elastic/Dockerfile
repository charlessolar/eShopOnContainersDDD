FROM microsoft/dotnet:2.0-sdk AS build

WORKDIR /src

COPY . .
RUN dotnet restore *.sln
WORKDIR /src/src/Endpoints/Elastic
RUN dotnet build --no-restore -c Release -o /app

#FROM build AS testrunner
#WORKDIR /src/tests
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]

#FROM build AS test
#WORKDIR /src/tests
#RUN dotnet test

FROM build AS publish
WORKDIR /src/src/Endpoints/Elastic
RUN dotnet publish --no-restore -c Release -o /app


FROM microsoft/dotnet:2.0-runtime AS runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Elastic.dll"]
