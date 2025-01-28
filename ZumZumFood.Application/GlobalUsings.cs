// 1. Thư viện hệ thống cơ bản
global using System.Text.RegularExpressions;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.ComponentModel.DataAnnotations;
global using System.Net.Mail;
global using System.Net;
global using System.Data;
global using JsonSerializer = System.Text.Json.JsonSerializer;


// 2. Thư viện liên quan đến bảo mật và xác thực
global using Microsoft.IdentityModel.Tokens;

// 3. Thư viện liên quan đến ASP.NET Core
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication;

// 4. Thư viện xử lý hình ảnh
global using SixLabors.ImageSharp;
global using SixLabors.ImageSharp.Formats.Webp;
global using Image = SixLabors.ImageSharp.Image;

// 5. Thư viện xử lý dữ liệu và cơ sở dữ liệu
global using Microsoft.EntityFrameworkCore;
global using StackExchange.Redis;

// 6. Thư viện từ bên thứ ba
global using AutoMapper;
global using X.PagedList;
global using Newtonsoft.Json;
global using RabbitMQ.Client;
global using System.Text;
global using RabbitMQ.Client.Events;
global using Microsoft.Extensions.Hosting;
global using Nest;

// 7. Thư viện của ứng dụng riêng (ZumZumFood)
global using ZumZumFood.Application.Abstracts;
global using ZumZumFood.Application.Models.Request;
global using ZumZumFood.Application.Models.Response;
global using ZumZumFood.Application.Models.DTOs;
global using ZumZumFood.Application.Models.RequestModel;
global using ZumZumFood.Application.Utils;
global using ZumZumFood.Domain.Abstracts;
global using ZumZumFood.Domain.Entities;
global using ZumZumFood.Infrastructure.Abstracts;
global using static ZumZumFood.Application.Utils.Helpers.Helpers;
global using ZumZumFood.Infrastructure.Services;
global using ZumZumFood.Application.Utils.Common;
global using ZumZumFood.Application.Utils.Helpers;
global using ZumZumFood.Application.Utils.Helpers.Token;
global using ZumZumFood.Application.Models.Queries.Components;
