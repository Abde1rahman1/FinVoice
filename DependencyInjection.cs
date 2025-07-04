﻿using FinVoice.Authentication;
using FinVoice.Database;
using FinVoice.Entities;
using FinVoice.Services.Analytics;
using FinVoice.Services.Auth;
using FinVoice.Services.BudgetService;
using FinVoice.Services.ExpenseService;
using FinVoice.Services.Notification;
using FinVoice.Settings;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FinVoice;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,
     IConfiguration configuration)
    {
        services.AddControllers();
        services.AddAuthConfig(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IVoiceAnalysisService, VoiceAnalysisService>();
        services.AddScoped<IManualExpenseService, ManualExpenseService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHttpClient<VoiceAnalysisService>();
        services.AddProblemDetails();
        services.AddBackgroundJobsConfig(configuration);
        services.AddHttpContextAccessor();
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        return services;
    }


    private static IServiceCollection AddAuthConfig (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtPorvicer, JwtPorvicer>();
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"]
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });


        return services;
    }
    private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();

        return services;
    }
}
