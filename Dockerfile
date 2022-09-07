FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source

COPY .  .
RUN dotnet restore 

COPY . .
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MyApp.dll"]  


EXPOSE 5000