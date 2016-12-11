FROM microsoft/dotnet

WORKDIR /meetup/Api

COPY NuGet.config /meetup
COPY ./src/Api /meetup/Api
COPY ./src/Domain /meetup/Domain

RUN dotnet restore

RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS http://+:5000
ENTRYPOINT ["dotnet", "./out/Api.dll"]