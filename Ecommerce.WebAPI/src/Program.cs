using System.Text;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Shared;
using Ecommerce.WebAPI.src.AuthorizationPolicy;
using Ecommerce.WebAPI.src.Database;
using Ecommerce.WebAPI.src.Middleware;
using Ecommerce.WebAPI.src.Repo;
using Ecommerce.WebAPI.src.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add all controllers
builder.Services.AddControllers();

// adding db context into project
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Localhost"));
dataSourceBuilder.MapEnum<UserRole>();
dataSourceBuilder.MapEnum<OrderStatus>();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AppDbContext>
(
      options =>
      options
        .UseNpgsql(dataSource)
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(new TimeStampInterceptor())
);

// service registration -> automatically create all instances of dependencies


builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IProductImageRepo, ProductImageRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<ExceptionHandlerMiddleware>(serviceProvider =>
{
  var logger = serviceProvider.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
  return new ExceptionHandlerMiddleware(next =>
  {
    var requestDelegate = serviceProvider.GetRequiredService<RequestDelegate>();
    return Task.CompletedTask;
  }, logger);
}); // Catching database exception

// Register authorization handler
builder.Services.AddSingleton<IAuthorizationHandler, VerifyResourceOwnerHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAccountHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerOrderHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerReviewHandler>();

// add authentication instructions
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true, // make sure it's not expired
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Secrets:Issuer"],
      };
    }
);

// Add authorization instructions
builder.Services.AddAuthorization(
    policy =>
    {
      policy.AddPolicy("ResourceOwner", policy => policy.Requirements.Add(new VerifyResourceOwnerRequirement()));
      policy.AddPolicy("AdminOrOwnerAccount", policy => policy.Requirements.Add(new AdminOrOwnerAccountRequirement()));
      policy.AddPolicy("AdminOrOwnerOrder", policy => policy.Requirements.Add(new AdminOrOwnerOrderRequirement()));
      policy.AddPolicy("AdminOrOwnerReview", policy => policy.Requirements.Add(new AdminOrOwnerReviewRequirement()));
    }
);



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();