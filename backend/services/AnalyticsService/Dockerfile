FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Install native dependencies for PDF rendering
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    fontconfig \
    libx11-dev \
    libxext6 \
    libxrender1 \
    libxtst6 \
    libxi6 \
    libjpeg-dev \
    && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll


COPY *.csproj ./
RUN dotnet restore


COPY . ./
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .


EXPOSE 8080

ENTRYPOINT ["dotnet", "AnalyticsService.dll"]
