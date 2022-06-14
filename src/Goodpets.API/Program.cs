using Goodpets.API.SeedWork.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApplicationPartManager(manager =>
    manager.FeatureProviders.Add(new InternalControllerFeatureProvider()));

builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.EnableAnnotations();
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Goodpets API",
        Version = "v1"
    });

    swagger?.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();