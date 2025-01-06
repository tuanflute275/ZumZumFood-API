// 1. Thư viện hệ thống cơ bản
global using System.Text.Json;

// 2. Thư viện ASP.NET Core
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.AspNetCore.Authentication.OAuth;
global using Microsoft.Extensions.Diagnostics.HealthChecks;

// 3. Thư viện xử lý cơ sở dữ liệu
global using Microsoft.EntityFrameworkCore;
global using StackExchange.Redis;

// 4. Thư viện bảo mật và xác thực
global using Microsoft.IdentityModel.Tokens;

// 5. Thư viện bên thứ ba
global using DinkToPdf;
global using DinkToPdf.Contracts;
global using Serilog;
global using ILogger = Serilog.ILogger;
global using RabbitMQ.Client;
global using System.Text;


// 6. Thư viện ứng dụng (ZumZumFood)
global using ZumZumFood.Application.Abstracts;
global using ZumZumFood.Application.Configuration;
global using ZumZumFood.Application.Models.Request;
global using ZumZumFood.Application.Models.Response;
global using ZumZumFood.Application.Services;
global using ZumZumFood.Application.Utils;
global using ZumZumFood.Domain.Abstracts;
global using ZumZumFood.Domain.Entities;
global using ZumZumFood.Infrastructure.Abstracts;
global using ZumZumFood.Infrastructure.Services;
global using ZumZumFood.Persistence.Data;
global using ZumZumFood.Persistence.Dapper;
global using ZumZumFood.Persistence.Repositories;
