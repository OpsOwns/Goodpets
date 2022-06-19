var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(x => x.Filters.Add(new FluentValidationAttribute())).ConfigureApplicationPartManager(
    manager =>
        manager.FeatureProviders.Add(new InternalControllerFeatureProvider())).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddHttpContextAccessor();
builder.Services.AddFluentValidation(FluentValidatorExtensions.AddConfiguration());

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
        Description = "Put **_ONLY_** your JWT Bearer token on textBox below!",
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

if (app.Environment.IsDevelopment())
{
    builder.Configuration
        .AddUserSecrets<Program>()
        .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
}
else
{
    builder.Configuration.AddAzureKeyVault(builder.Configuration["AzureKeyVault:Url"],
        builder.Configuration["AzureKeyVault:ClientId"],
        builder.Configuration["AzureKeyVault:SecretKey"]);
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context => context.Response.WriteAsync("Goodpets API Gateway"));

await app.RunAsync();