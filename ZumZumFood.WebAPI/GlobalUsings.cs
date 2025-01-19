// 1. Thư viện liên quan đến ASP.NET Core
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Facebook;
global using Microsoft.AspNetCore.Authentication.Google;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// 2. Thư viện của ứng dụng riêng (ZumZumFood)
global using ZumZumFood.Application.Abstracts;
global using ZumZumFood.Application.Models.Request;
global using ZumZumFood.Application.Models.Response;
global using ZumZumFood.Application.Models.RequestModel;
global using ZumZumFood.Infrastructure.Configuration;
global using ZumZumFood.Application.Utils.Helpers.Token;

// 3. Thư viện khác
global using System.Text.Json;
global using Serilog;