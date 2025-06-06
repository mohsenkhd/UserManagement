using Application.RepositoryContracts.ApplicationAggregate;
using Application.RepositoryContracts.ClientAggregate;
using Application.RepositoryContracts.HubLogAggregate;
using Application.RepositoryContracts.InvoiceAggregate;
using Application.RepositoryContracts.OrderAggregate;
using Application.RepositoryContracts.PermissionAggregate;
using Application.RepositoryContracts.RoleAggregate;
using Application.RepositoryContracts.UserAggregate;
using Application.RepositoryContracts.UsersLoginHistoryAggregate;
using Application.RepositoryContracts.WalletAggregate;
using Application.ServiceContracts.ApplicationAggregate;
using Application.ServiceContracts.Captcha;
using Application.ServiceContracts.ClientAggregate;
using Application.ServiceContracts.Fundamental;
using Application.ServiceContracts.HubLogAggregate;
using Application.ServiceContracts.InvoiceAggregate;
using Application.ServiceContracts.Notifier;
using Application.ServiceContracts.OrderAggregate;
using Application.ServiceContracts.Otp;
using Application.ServiceContracts.PermissionService;
using Application.ServiceContracts.RoleAggregate;
using Application.ServiceContracts.UserAggregate;
using Application.ServiceContracts.UsersLoginHistory;
using Application.ServiceContracts.WalletAggregate;
using Common.Helper;
using Common.Wrappers;
using Context.DataBaseContext;
using Context.DataSeeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.ApplicationAggregate;
using Repository.ClientAggregate;
using Repository.HubLogAggregate;
using Repository.InvoiceAggregate;
using Repository.OrderAggregate;
using Repository.PermissionAggregate;
using Repository.RoleAggregate;
using Repository.UserAggregate;
using Repository.UsersLoginHistoryAggregate;
using Repository.WalletAggregate;
using Service.ApplicationAggregate;
using Service.Captcha;
using Service.ClientAggregate;
using Service.Fundamental;
using Service.HubLogAggregate;
using Service.InvoiceAggregate;
using Service.Notifier;
using Service.OrderAggregate;
using Service.Otp;
using Service.PermissionAggregate;
using Service.RoleAggregate;
using Service.UserAggregate;
using Service.UsersLoginHistoryAggregate;
using Service.WalletAggregate;
using StackExchange.Redis;
using System.Text;
using System.Text.Json.Serialization;
using UserManagement.Filters;
using UserManagement.Hub;

var builder = WebApplication.CreateBuilder(args);
const string allowSpecificOrigins = "_allowSpecificOrigins";
const string httpClient = "HttpClient";
const string swaggerVersion = "v3";
const string swaggerTitle = "UserManagement";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        corsPolicyBuilder =>
        {
            var origins = builder.Configuration.GetSection("Origins").Get<List<string>>();
            foreach (var origin in origins)
            {
                corsPolicyBuilder.WithOrigins(origin);
            }
            corsPolicyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .Build();
        });
});
// Add services to the container.
var multiplexer = ConnectionMultiplexer.Connect($"{builder.Configuration["Redis:Server"]}:{builder.Configuration["Redis:Port"]}",
    opt =>
    {
        opt.AbortOnConnectFail = false;
    });
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

builder.Services.AddSignalR();
builder.Services.AddControllers(o => { o.Filters.Add(typeof(ExceptionFilter)); o.Filters.Add(typeof(TransactionFilter)); })
    .ConfigureApiBehaviorOptions(op =>
    {
        op.InvalidModelStateResponseFactory = context =>
        {
            return new ObjectResult(new
            {
                ClientMessage = string.Join("\n",
                    context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                Code = -800
            })
            {
                StatusCode = 400
            };
        };
    })
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<CaptchaWrapper>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<INotifierService, NotifierService>();
builder.Services.AddScoped<IFundamentalService, FundamentalService>();
builder.Services.AddScoped<IUsersLoginHistoryRepository, UsersLoginHistoryRepository>();
builder.Services.AddScoped<IUsersLoginHistoryService, UsersLoginHistoryService>();
builder.Services.AddScoped<IHubLogService, HubLogService>();
builder.Services.AddScoped<IHubLogRepository, HubLogRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();


builder.Services.AddSignalR();
builder.Services.AddHttpClient(httpClient, client => { client.Timeout = new TimeSpan(0, 0, 10); });

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<UserManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"),
        b => b
            .MigrationsAssembly("Context")
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Titec.Core.Bearer")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(swaggerVersion, new OpenApiInfo { Title = swaggerTitle, Version = swaggerVersion });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Access Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        scope.ServiceProvider.GetRequiredService<UserManagementContext>().Database.Migrate();
        ApplicationSeedData.Initialize(services);
        PermissionSeedData.Initialize(services);
        UserSeedData.Initialize(services);
        RoleSeedData.Initialize(services);
        UserRoleSeedData.Initialize(services);
        RolePermissionSeedData.Initialize(services);
        WalletSeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.

app.UseSwagger(opt =>
{
    opt.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        var subPath = builder.Configuration["Swagger:SubPath"];
        if (string.IsNullOrEmpty(subPath)) return;
        var serverUrl = $"https://{httpReq.Host}/{subPath}";
        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
    });
});
app.UseSwaggerUI(opt =>
{
    var subPath = builder.Configuration["Swagger:SubPath"];
    opt.SwaggerEndpoint(
        !string.IsNullOrEmpty(subPath)
            ? $"/{subPath}swagger/{swaggerVersion}/swagger.json"
            : $"/swagger/{swaggerVersion}/swagger.json",
        $"{swaggerTitle} Api ({swaggerVersion})");
});
app.MapHub<OnlineUserHub>("/onlineUser");
app.UseCors(allowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();