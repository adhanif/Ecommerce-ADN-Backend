# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS first-stage
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Ecommerce.Controller/Ecommerce.Controller.csproj Ecommerce.Controller/
COPY Ecommerce.Core/Ecommerce.Core.csproj Ecommerce.Core/
COPY Ecommerce.Service/Ecommerce.Service.csproj Ecommerce.Service/
COPY Ecommerce.WebAPI/Ecommerce.WebAPI.csproj Ecommerce.WebAPI/
RUN dotnet restore

# copy everything else and build app
COPY Ecommerce.Controller/ Ecommerce.Controller/
COPY Ecommerce.Core/ Ecommerce.Core/
COPY Ecommerce.Service/ Ecommerce.Service/
COPY Ecommerce.WebAPI/ Ecommerce.WebAPI/
WORKDIR /source/Ecommerce.WebAPI
RUN dotnet publish -c release -o /app
### 



# second stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=first-stage /app ./
EXPOSE 8080
CMD ["dotnet", "Ecommerce.WebAPI.dll"]