#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["1. Web/MoreThanBlog/MoreThanBlog.csproj", "1. Web/MoreThanBlog/"]
COPY ["3. Repository/Repository/Repository.csproj", "3. Repository/Repository/"]
COPY ["3. Repository/Abstraction.Repository/Abstraction.Repository.csproj", "3. Repository/Abstraction.Repository/"]
COPY ["4. Cross/Core/Core.csproj", "4. Cross/Core/"]
COPY ["4. Cross/Mapper/Mapper.csproj", "4. Cross/Mapper/"]
COPY ["2.Service/Service/Service.csproj", "2.Service/Service/"]
COPY ["2.Service/Abstraction.Service/Abstraction.Service.csproj", "2.Service/Abstraction.Service/"]
RUN dotnet restore "1. Web/MoreThanBlog/MoreThanBlog.csproj"
COPY . .
WORKDIR "/src/1. Web/MoreThanBlog"
RUN dotnet build "MoreThanBlog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoreThanBlog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoreThanBlog.dll"]