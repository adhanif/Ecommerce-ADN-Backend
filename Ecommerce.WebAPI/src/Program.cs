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
using Microsoft.OpenApi.Models;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);




// Add CORS services
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
    builder =>
    {
      builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
      options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
      {
        Description = "Bearer token authentication",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
      }
      );

      // swagger would add the token to the request header of routes with [Authorize] attribute
      options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);



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


builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);


// add DI services
builder.Services
.AddScoped<IProductRepo, ProductRepo>()
.AddScoped<IProductImageRepo, ProductImageRepo>()
.AddScoped<IUserRepo, UserRepo>()
.AddScoped<ICategoryRepo, CategoryRepo>()
.AddScoped<IOrderRepo, OrderRepo>()
.AddScoped<IReviewRepo, ReviewRepo>()
.AddScoped<IAddressRepo, AddressRepo>();


builder.Services.AddScoped<IUserService, UserService>()
.AddScoped<IProductService, ProductService>()
.AddScoped<ICategoryService, CategoryService>()
.AddScoped<IOrderService, OrderService>()
.AddScoped<IReviewService, ReviewService>()
.AddScoped<IAuthService, AuthService>()
.AddScoped<ITokenService, TokenService>()
.AddScoped<IPasswordService, PasswordService>()
.AddScoped<IAddressService, AddressService>();



// add DI custom authorization services
builder.Services.AddSingleton<IAuthorizationHandler, VerifyResourceOwnerHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAccountHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerOrderHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrOwnerReviewHandler>();



builder.Services.AddScoped<ExceptionHandlerMiddleware>(serviceProvider =>
{
  var logger = serviceProvider.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
  return new ExceptionHandlerMiddleware(next =>
  {
    var requestDelegate = serviceProvider.GetRequiredService<RequestDelegate>();
    return Task.CompletedTask;
  }, logger);
}); // Catching database exception



// add authentication instructions
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidIssuer = builder.Configuration["Secrets:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true, // make sure it's not expired
        ValidateIssuerSigningKey = true,
      };
    }
);

// Add authorization instructions
builder.Services.AddAuthorization(
    policy =>
    {
      policy.AddPolicy("ResourceOwnerAddress", policy => policy.Requirements.Add(new VerifyResourceOwnerAddressRequirement()));
      policy.AddPolicy("ResourceOwner", policy => policy.Requirements.Add(new VerifyResourceOwnerRequirement()));
      policy.AddPolicy("AdminOrOwnerAccount", policy => policy.Requirements.Add(new AdminOrOwnerAccountRequirement()));
      policy.AddPolicy("AdminOrOwnerOrder", policy => policy.Requirements.Add(new AdminOrOwnerOrderRequirement()));
      policy.AddPolicy("AdminOrOwnerReview", policy => policy.Requirements.Add(new AdminOrOwnerReviewRequirement()));
    }
);



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
  // options.RoutePrefix = string.Empty; // "/swagger/index.html"
});



app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();