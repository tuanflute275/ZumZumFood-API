# Sử dụng hình ảnh .NET 8 ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8000

# Sử dụng hình ảnh .NET 8 SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép toàn bộ solution và các project .csproj
COPY ["ZumZumFood.sln", "./"]

# Sao chép tất cả các tệp .csproj từ tất cả các thư mục con
COPY ["ZumZumFood.Domain/ZumZumFood.Domain.csproj", "ZumZumFood.Domain/"]
COPY ["ZumZumFood.Persistence/ZumZumFood.Persistence.csproj", "ZumZumFood.Persistence/"]
COPY ["ZumZumFood.Application/ZumZumFood.Application.csproj", "ZumZumFood.Application/"]
COPY ["ZumZumFood.Infrastructure/ZumZumFood.Infrastructure.csproj", "ZumZumFood.Infrastructure/"]
COPY ["ZumZumFood.WebAPI/ZumZumFood.WebAPI.csproj", "ZumZumFood.WebAPI/"]

# Khôi phục các gói NuGet cần thiết
RUN dotnet restore "ZumZumFood.sln"

# Sao chép toàn bộ mã nguồn dự án vào container
COPY . .

# Build ứng dụng
RUN dotnet build "ZumZumFood.sln" -c Release -o /app/build

# Tạo bước publish
FROM build AS publish
RUN dotnet publish "ZumZumFood.WebAPI/ZumZumFood.WebAPI.csproj" -c Release -o /app/publish

# Sử dụng runtime để chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "ZumZumFood.WebAPI.dll"]

# docker build -t zumzum-food:1.0.0 -f ./Dockerfile .
# docker tag zumzum-food:1.0.0 tuanflute/zumzum-food:1.0.0
# docker push tuanflute/zumzum-food:1.0.0