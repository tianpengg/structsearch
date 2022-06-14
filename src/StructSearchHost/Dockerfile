#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY ["StructSearchHost/StructSearchHost.csproj", "StructSearchHost/"]
#COPY ["Struct.Module.Business/Struct.Module.Business.csproj", "Struct.Module.Business/"]
#COPY ["Struct.Models.Dto/Struct.Models.Dto.csproj", "Struct.Models.Dto/"]
#COPY ["Struct.Common.Utils/Struct.Common.Utils.csproj", "Struct.Common.Utils/"]
#COPY ["Struct.Models.Help/Struct.Models.Help.csproj", "Struct.Models.Help/"]
#COPY ["Struct.Module.IBusiness/Struct.Module.IBusiness.csproj", "Struct.Module.IBusiness/"]
#RUN dotnet restore "StructSearchHost/StructSearchHost.csproj"
#COPY . .
#WORKDIR "/src/StructSearchHost"
#RUN dotnet build "StructSearchHost.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "StructSearchHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "StructSearchHost.dll"]