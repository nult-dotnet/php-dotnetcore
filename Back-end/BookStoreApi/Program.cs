using BookStoreApi.Services;
using BookStoreApi.Settings;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//log file
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Logging.AddConsole();

builder.Services.Configure<BookStoreDatabaseSetting>(builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<RolesService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<BillsService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCaching();
//Add JsonPatch
builder.Services.AddControllers().AddNewtonsoftJson();
//Config AutoMapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
                builder =>
                {
                    builder.WithOrigins("https://localhost:44313/")
                            .WithMethods("PUT","DELETE", "GET","POST");
                });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();