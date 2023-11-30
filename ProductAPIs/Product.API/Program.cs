using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.API.AppService.Contracts;
using Product.API.AppService.Implementations;
using Product.Core;
using Product.Core.Configuration;
using Product.Core.Data.IRepository;
using Product.Persistence;
using Product.Persistence.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.



builder.Services.AddDbContext<ProductDbContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductCoreProgram).Assembly));


builder.Services.Configure<StoredProducerTitles>(configuration.GetSection("StoredProducerTitles"));


//Authentification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:7111";
    options.Audience = "ProductApi";
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SecretKey").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero

    };
});


builder.Services.AddCors(options =>
{
    var allowCors = configuration.GetSection("AllowCors").Value;
    if (allowCors != null)
        options.AddPolicy("MyCorsPolicy", builder =>
        {
            builder.WithOrigins(allowCors)
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddTransient(typeof(IProductAppService), typeof(ProductAppService));
builder.Services.AddTransient<ISpProductAppService, SpProductAppService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ISpProductRepository, SpProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Disable SSL certificate validation in development
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("MyCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
