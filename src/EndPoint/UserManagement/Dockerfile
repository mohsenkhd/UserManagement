FROM 10.227.222.197:30001/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM 10.227.222.197:30001/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EndPoint/UserManagement/UserManagement.csproj", "src/EndPoint/UserManagement/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/Infrastructure/Repository/Repository.csproj", "src/Infrastructure/Repository/"]
COPY ["src/Core/Common/Common.csproj", "src/Core/Common/"]
COPY ["src/Infrastructure/Context/Context.csproj", "src/Infrastructure/Context/"]
COPY ["src/Infrastructure/Service/Service.csproj", "src/Infrastructure/Service/"]
RUN dotnet restore "src/EndPoint/UserManagement/UserManagement.csproj"
COPY . .
WORKDIR "/src/src/EndPoint/UserManagement"
RUN dotnet build "UserManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.dll"]
