FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia e restaura as dependencias
COPY ["BibliotecaWeb.csproj", "./"]
RUN dotnet restore "BibliotecaWeb.csproj"

# Copia o restante dos arquivos e realiza a publicacao
COPY . .
RUN dotnet publish "BibliotecaWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estagio final de execucao
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configuracao da porta padrao para o Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BibliotecaWeb.dll"]