FROM microsoft/dotnet:sdk

WORKDIR /meetup/Consumer

COPY NuGet.config /meetup
COPY ./src/Consumer /meetup/Consumer
COPY ./src/Domain /meetup/Domain

RUN dotnet restore
RUN dotnet publish -c Release -o out

ENTRYPOINT ["dotnet", "./out/Consumer.dll"]