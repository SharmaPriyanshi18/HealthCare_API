using HealthCare_API;
using HealthCareData.Identity;
using HealthCareRepositorys.Repository.IRepository;
using HealthCareRepositorys.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HealthCareData;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hangfire;
using Hangfire.SqlServer;
using HealthCareModels.MappingProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);
var appSettings = appSettingSection.Get<AppSettings>();

string cs = builder.Configuration.GetConnectionString("conStr");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(cs, b => b.MigrationsAssembly("HealthCare_API")));

builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
builder.Services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
//builder.Services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddTransient<PendingEmailSender>();


builder.Services.Configure<emailSetting>(builder.Configuration.GetSection("emailSetting"));
builder.Services.AddTransient<IEmailSender, EmailSender>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<ApplicationUserStore>()
    .AddUserManager<ApplicationUserManager>()
    .AddRoleManager<ApplicationRoleManager>()
    .AddRoleStore<ApplicationRoleStore>()
    .AddDefaultTokenProviders();

// Hangfire Configuration
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(cs, new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.FromSeconds(15),
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          }));

builder.Services.AddHangfireServer();



builder.Services.AddScoped<ApplicationRoleStore>();
builder.Services.AddScoped<ApplicationUserStore>();

builder.Services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
builder.Services.AddScoped<IUserServiceRepository, userServiceRepository>();
builder.Services.AddScoped<ITherapistRepository, TherapistRepository>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<IDiseaseRepository, DiseaseRepository>();
builder.Services.AddScoped<IShedularRepository, ShedularRepository>();
builder.Services.AddScoped<ISchedulerQueueService, SchedulerQueueService>();
//builder.Services.AddHostedService<PendingEmailSender>();





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "HealthCare API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token.\nExample: Bearer <your token>"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// JWT Authentication
var key = Encoding.ASCII.GetBytes(appSettings.secret);
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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token validation failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("Unauthorized access: " + context.ErrorDescription);
            return Task.CompletedTask;
        }
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
//builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Seed Roles and Default Users
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    string[] roles = new[] { "Admin", "Patient" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new ApplicationRole { Name = role });
//        }
//    }

//    // Create Admin user
//    if (await userManager.FindByNameAsync("sacchu") == null)
//    {
//        var admin = new ApplicationUser
//        {
//            UserName = "sacchu",
//            Email = "sachu@gmail.com",
//            Address = "Admin Default Address",
//            City = "Admin City",
//            Country = "India",
//            State = "Delhi",
//            PostalCode = "110001"
//        };
//        var result = await userManager.CreateAsync(admin, "Admin123#");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(admin, "Admin");
//        }
//    }

//    // Create Patient 
//    if (await userManager.FindByNameAsync("priyanshi") == null)
//    {
//        var user = new ApplicationUser
//        {
//            UserName = "priyanshi",
//            Email = "priyanshi@gmail.com",
//            Address = "Patient Default Address",
//            City = "Patient City",
//            Country = "India",
//            State = "Madhya Pradesh",
//            PostalCode = "482002"
//        };
//        var result = await userManager.CreateAsync(user, "Users123#");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(user, "Patient");
//        }
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<PendingEmailSender>(
    "send-scheduler-emails",
    sender => sender.SendPendingEmailsAsync(),
    "0 10 * * *", 
    TimeZoneInfo.Local
);
app.Run();
