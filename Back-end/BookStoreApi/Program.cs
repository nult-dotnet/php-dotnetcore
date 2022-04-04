using Autofac;
using Autofac.Extensions.DependencyInjection;
using BookStoreApi.Autofac;
using BookStoreApi.DBContext;
using Serilog;
using Serilog.Filters;
var builder = WebApplication.CreateBuilder(args); 

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

//Autofac Custom dependency injection (DI) container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModule()));

//Config bookStotedatabase

builder.Services.AddDbContext<SQLContext>();
//Dependency config

/*builder.Services.AddScoped<IRoleService,RolesService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IBookService,BooksService>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IBillService,BillsService>();
builder.Services.AddScoped<ILogService,LogsService>();
*/

//AddController
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
//Add JsonPatch
builder.Services.AddControllers().AddNewtonsoftJson();

//Add AutoMapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add SignalR
builder.Services.AddSignalR();
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