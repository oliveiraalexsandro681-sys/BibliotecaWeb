FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia e restaura o ficheiro .csproj apontando para a pasta BibliotecaWeb
COPY ["BibliotecaWeb/BibliotecaWeb.csproj", "BibliotecaWeb/"]
RUN dotnet restore "BibliotecaWeb/BibliotecaWeb.csproj"

# Copia todo o conteudo e compila
COPY . .
WORKDIR "/src/BibliotecaWeb"
RUN dotnet publish "BibliotecaWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estagio final de execucao
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BibliotecaWeb.dll"]