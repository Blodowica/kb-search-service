using dotenv.net;
using SearchingService_POC_c_.Interfaces;
using SearchingService_POC_c_.Models;
using SearchingService_POC_c_.Services;

var builder = WebApplication.CreateBuilder(args);
 
DotEnv.Load();  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//The movie database config
builder.Services.Configure<TMDbSettings>(builder.Configuration.GetSection("TMDb"));
builder.Services.AddScoped<MovieService>();


builder.Configuration.AddEnvironmentVariables();

var token = builder.Configuration["TMDb:BearerToken"];
Console.WriteLine($"Token: {token}");

//Cors configuration
builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedHosts", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//interfaces 
builder.Services.AddScoped<IMovieService, MovieService>();

//Build application
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowedHosts");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
