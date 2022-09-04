#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80  
EXPOSE 443 
ENV ASPNETCORE_URLS=http://*:5000


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["com.tweetapp.api/com.tweetapp.api.csproj", "com.tweetapp.api/"]
RUN dotnet restore "com.tweetapp.api/com.tweetapp.api.csproj"
COPY . .
WORKDIR "/src/com.tweetapp.api"
RUN dotnet build "com.tweetapp.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "com.tweetapp.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "com.tweetapp.api.dll"]