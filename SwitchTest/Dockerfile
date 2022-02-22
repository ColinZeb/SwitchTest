#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
ENV TZ=Asia/Shanghai
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SwitchTest/SwitchTest.csproj", "SwitchTest/"]
RUN dotnet restore "SwitchTest/SwitchTest.csproj"
COPY . .
WORKDIR "/src/SwitchTest"
RUN dotnet build "SwitchTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SwitchTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SwitchTest.dll"]