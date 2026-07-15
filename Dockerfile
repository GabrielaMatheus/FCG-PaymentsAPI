FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/FiapCloudGames.PaymentsApi/FiapCloudGames.PaymentsApi.csproj src/FiapCloudGames.PaymentsApi/
RUN dotnet restore src/FiapCloudGames.PaymentsApi/FiapCloudGames.PaymentsApi.csproj
COPY src/FiapCloudGames.PaymentsApi/ src/FiapCloudGames.PaymentsApi/
RUN dotnet publish src/FiapCloudGames.PaymentsApi/FiapCloudGames.PaymentsApi.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN adduser --disabled-password --gecos "" --home /app appuser && chown -R appuser:appuser /app
USER appuser
COPY --from=build --chown=appuser:appuser /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FiapCloudGames.PaymentsApi.dll"]
