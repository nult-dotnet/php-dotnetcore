using BookStoreApi.Interfaces;
using BookStoreApi.Services;
using BookStoreApi.Settings;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});
// Add services to the container.
//Config log file
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Filter.ByExcluding(Matching.FromSource("Microsoft"))
  .Filter.ByExcluding(Matching.FromSource("System"))
  .Enrich.FromLogContext()
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
builder.Services.AddSingleton<LogsService>();

//Dependency config
builder.Services.AddScoped<IRoleService,RolesService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IBookService,BooksService>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IBillService,BillsService>();
builder.Services.AddScoped<ILogService,LogsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCaching();
//Add JsonPatch
builder.Services.AddControllers().AddNewtonsoftJson();
//Config AutoMapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
app.UseStaticFiles();
app.MapControllers();
app.UseHttpLogging();
app.Run();