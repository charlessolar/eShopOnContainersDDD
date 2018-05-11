FROM microsoft/aspnetcore-build:2.0 AS build

WORKDIR /src

COPY . .
RUN dotnet restore *.sln
WORKDIR /src/src/Endpoints/Presentation
RUN dotnet build --no-restore -c Release -o /app

#FROM build AS testrunner
#WORKDIR /src/tests
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]

#FROM build AS test
#WORKDIR /src/tests
#RUN dotnet test

FROM build AS publish
WORKDIR /src/src/Endpoints/Presentation
RUN dotnet publish --no-restore -c Release -o /app


FROM microsoft/aspnetcore:2.0 AS runtime
EXPOSE 80
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Presentation.dll"]
