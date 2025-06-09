FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Add this if you're using PDF generation that relies on System.Drawing.Common
RUN apt-get update && apt-get install -y libgdiplus libc6-dev


COPY *.csproj ./
RUN dotnet restore


COPY . ./
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .


EXPOSE 8080

ENTRYPOINT ["dotnet", "AnalyticsService.dll"]
