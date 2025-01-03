﻿using System.Text;
using System.Text.Json.Serialization;
using EMS.BL.Repositories;
using EMS.BL.Services;
using EMS.Database.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	options.JsonSerializerOptions.WriteIndented = true; // Nếu muốn format JSON đẹp hơn (có thụt dòng)
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var secret = builder.Configuration.GetValue<string>("Jwt:Secret");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "TranBao",
        ValidAudience = "TranBao",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
builder.Services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRepository>();

builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

builder.Services.AddScoped<IRotatingRequestService, RotatingRequestService>();
builder.Services.AddScoped<IRotatingRequestRepository, RotatingRequestRepository>();

builder.Services.AddScoped<IRotatingHistoryService, RotatingHistoryService>();
builder.Services.AddScoped<IRotatingHistoryRepository, RotatingHistoryRepository>();

builder.Services.AddScoped<IPurchasingRequestService, PurchasingRequestService>();
builder.Services.AddScoped<IPurchasingRequestRepository, PurchasingRequestRepository>();

builder.Services.AddScoped<IPurchasingHistoryService, PurchasingHistoryService>();
builder.Services.AddScoped<IPurchasingHistoryRepository, PurchasingHistoryRepository>();

builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
builder.Services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();

builder.Services.AddScoped<IMaintenanceHistoryService, MaintenanceHistoryService>();
builder.Services.AddScoped<IMaintenanceHistoryRepository, MaintenanceHistoryRepository>();

builder.Services.AddScoped<ILiquidationRequestService, LiquidationRequestService>();
builder.Services.AddScoped<ILiquidationRequestRepository, LiquidationRequestRepository>();

builder.Services.AddScoped<ILiquidationHistoryService, LiquidationHistoryService>();
builder.Services.AddScoped<ILiquidationHistoryRepository, LiquidationHistoryRepository>();

// Add services to the container.
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

app.Run();

