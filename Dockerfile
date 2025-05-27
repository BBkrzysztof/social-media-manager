FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
RUN dotnet tool install --global dotnet-watch
ENV PATH="$PATH:/root/.dotnet/tools"


ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true

CMD ["dotnet", "watch", "run", "--project", "/app", "--urls", "http://0.0.0.0:5000"]
