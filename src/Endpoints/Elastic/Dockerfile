FROM microsoft/dotnet:2.0-sdk AS build

WORKDIR /src

# only copy the projects so restore layer isnt re-done for source changes
COPY *.sln .
COPY src/Common.props ./src/
# ugliness needed until I figure out a better way to copy only project files or do a dotnet restore on not everything
COPY src/Contexts/Basket/Applications/MongoDb/*.csproj ./src/Contexts/Basket/Applications/MongoDb/
COPY src/Contexts/Basket/Domain/Handlers/*.csproj ./src/Contexts/Basket/Domain/Handlers/
COPY src/Contexts/Basket/Domain/Entities/*.csproj ./src/Contexts/Basket/Domain/Entities/
COPY src/Contexts/Basket/Presentation/ServiceStack/*.csproj ./src/Contexts/Basket/Presentation/ServiceStack/
COPY src/Contexts/Basket/Language/*.csproj ./src/Contexts/Basket/Language/
COPY src/Contexts/Catalog/Applications/Elastic/*.csproj ./src/Contexts/Catalog/Applications/Elastic/
COPY src/Contexts/Catalog/Applications/MongoDb/*.csproj ./src/Contexts/Catalog/Applications/MongoDb/
COPY src/Contexts/Catalog/Domain/Handlers/*.csproj ./src/Contexts/Catalog/Domain/Handlers/
COPY src/Contexts/Catalog/Domain/Entities/*.csproj ./src/Contexts/Catalog/Domain/Entities/
COPY src/Contexts/Catalog/Presentation/ServiceStack/*.csproj ./src/Contexts/Catalog/Presentation/ServiceStack/
COPY src/Contexts/Catalog/Language/*.csproj ./src/Contexts/Catalog/Language/
COPY src/Contexts/Configuration/Applications/MongoDb/*.csproj ./src/Contexts/Configuration/Applications/MongoDb/
COPY src/Contexts/Configuration/Domain/Handlers/*.csproj ./src/Contexts/Configuration/Domain/Handlers/
COPY src/Contexts/Configuration/Domain/Entities/*.csproj ./src/Contexts/Configuration/Domain/Entities/
COPY src/Contexts/Configuration/Presentation/ServiceStack/*.csproj ./src/Contexts/Configuration/Presentation/ServiceStack/
COPY src/Contexts/Configuration/Language/*.csproj ./src/Contexts/Configuration/Language/
COPY src/Contexts/Identity/Applications/Elastic/*.csproj ./src/Contexts/Identity/Applications/Elastic/
COPY src/Contexts/Identity/Applications/MongoDb/*.csproj ./src/Contexts/Identity/Applications/MongoDb/
COPY src/Contexts/Identity/Domain/Handlers/*.csproj ./src/Contexts/Identity/Domain/Handlers/
COPY src/Contexts/Identity/Domain/Entities/*.csproj ./src/Contexts/Identity/Domain/Entities/
COPY src/Contexts/Identity/Presentation/ServiceStack/*.csproj ./src/Contexts/Identity/Presentation/ServiceStack/
COPY src/Contexts/Identity/Language/*.csproj ./src/Contexts/Identity/Language/
COPY src/Contexts/Location/Applications/Elastic/*.csproj ./src/Contexts/Location/Applications/Elastic/
COPY src/Contexts/Location/Applications/MongoDb/*.csproj ./src/Contexts/Location/Applications/MongoDb/
COPY src/Contexts/Location/Domain/Handlers/*.csproj ./src/Contexts/Location/Domain/Handlers/
COPY src/Contexts/Location/Domain/Entities/*.csproj ./src/Contexts/Location/Domain/Entities/
COPY src/Contexts/Location/Presentation/ServiceStack/*.csproj ./src/Contexts/Location/Presentation/ServiceStack/
COPY src/Contexts/Location/Language/*.csproj ./src/Contexts/Location/Language/
COPY src/Contexts/Marketing/Applications/Elastic/*.csproj ./src/Contexts/Marketing/Applications/Elastic/
COPY src/Contexts/Marketing/Applications/MongoDb/*.csproj ./src/Contexts/Marketing/Applications/MongoDb/
COPY src/Contexts/Marketing/Domain/Handlers/*.csproj ./src/Contexts/Marketing/Domain/Handlers/
COPY src/Contexts/Marketing/Domain/Entities/*.csproj ./src/Contexts/Marketing/Domain/Entities/
COPY src/Contexts/Marketing/Presentation/ServiceStack/*.csproj ./src/Contexts/Marketing/Presentation/ServiceStack/
COPY src/Contexts/Marketing/Language/*.csproj ./src/Contexts/Marketing/Language/
COPY src/Contexts/Ordering/Applications/Elastic/*.csproj ./src/Contexts/Ordering/Applications/Elastic/
COPY src/Contexts/Ordering/Applications/MongoDb/*.csproj ./src/Contexts/Ordering/Applications/MongoDb/
COPY src/Contexts/Ordering/Domain/Handlers/*.csproj ./src/Contexts/Ordering/Domain/Handlers/
COPY src/Contexts/Ordering/Domain/Entities/*.csproj ./src/Contexts/Ordering/Domain/Entities/
COPY src/Contexts/Ordering/Presentation/ServiceStack/*.csproj ./src/Contexts/Ordering/Presentation/ServiceStack/
COPY src/Contexts/Ordering/Language/*.csproj ./src/Contexts/Ordering/Language/
COPY src/Contexts/Payment/Domain/Handlers/*.csproj ./src/Contexts/Payment/Domain/Handlers/
COPY src/Contexts/Payment/Domain/Entities/*.csproj ./src/Contexts/Payment/Domain/Entities/
COPY src/Contexts/Payment/Presentation/ServiceStack/*.csproj ./src/Contexts/Payment/Presentation/ServiceStack/
COPY src/Contexts/Payment/Language/*.csproj ./src/Contexts/Payment/Language/
COPY src/Endpoints/Domain/*.csproj ./src/Endpoints/Domain/
COPY src/Endpoints/MongoDb/*.csproj ./src/Endpoints/MongoDb/
COPY src/Endpoints/Elastic/*.csproj ./src/Endpoints/Elastic/
COPY src/Endpoints/Presentation/*.csproj ./src/Endpoints/Presentation/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/

RUN dotnet restore

COPY . .
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
