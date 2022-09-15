


FROM  mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ClientApp ClientApp

RUN apt-get update -yq
RUN apt-get install curl gnupg -yq
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install nodejs -yq

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/ClientApp ./ClientApp
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "RestaurantReview.dll"]