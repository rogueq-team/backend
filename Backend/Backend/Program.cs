using System.Security.Claims;
using System.Text;
using Backend;
using Backend.Hubs;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS первым делом
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // ОБЯЗАТЕЛЬНО для SignalR!
              .SetIsOriginAllowed(_ => true);
    });
});

// 2. SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    options.MaximumReceiveMessageSize = 1024 * 1024;
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"));
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<DealService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<FeedbackService>();

// 3. JWT для SignalR (ОЧЕНЬ ВАЖНО!)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:NoSecretKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateActor = false,
            RoleClaimType = "Role",
            NameClaimType = "UserId"
        };

        // Ключевая настройка для SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // 1. Проверяем Query String для WebSocket
                var accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                    return Task.CompletedTask;
                }

                // 2. Проверяем заголовок Authorization
                var tokenFromHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (!string.IsNullOrEmpty(tokenFromHeader))
                {
                    if (tokenFromHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = tokenFromHeader.Substring("Bearer ".Length).Trim();
                    }
                    else
                    {
                        context.Token = tokenFromHeader.Trim();
                    }
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Auth failed: {context.Exception?.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 4. ПРАВИЛЬНЫЙ ПОРЯДОК MIDDLEWARE
app.UseRouting();

// CORS ДО Authentication
app.UseCors("AllowAll");

// WebSockets ДО Authentication
app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Swagger на корне
    });
}

// 5. Map endpoints
app.MapControllers();

app.MapHub<ChatHub>("/chatHub", options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
    options.MinimumProtocolVersion = 0;
});

// Тестовые endpoints для проверки
app.MapGet("/", () => "Backend is running!");
app.MapGet("/chatHub/test", () => Results.Ok(new
{
    status = "SignalR endpoint is available",
    timestamp = DateTime.UtcNow,
    endpoint = "/chatHub"
}));

// Проверка negotiate
app.MapGet("/chatHub/negotiate", (HttpContext context) =>
{
    return Results.Ok(new
    {
        url = $"{context.Request.Scheme}://{context.Request.Host}/chatHub",
        accessToken = "test-token-placeholder",
        availableTransports = new[] { "WebSockets", "LongPolling" }
    });
});

try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        Console.WriteLine("База данных создана и миграции применены");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при создании БД: {ex.Message}");
}

Console.WriteLine("=========================================");
Console.WriteLine("Приложение запущено!");
Console.WriteLine($"SignalR Hub: /chatHub");
Console.WriteLine($"Swagger: /swagger");
Console.WriteLine($"Health check: /chatHub/test");
Console.WriteLine("=========================================");

app.Run();