using Autofac;
using Autofac.Extensions.DependencyInjection;
using BookStoreApi;
using BookStoreApi.Authenticate;
using BookStoreApi.Autofac;
using BookStoreApi.DataAccess.AutoCreateDB;
using BookStoreApi.DBContext;
using BookStoreApi.SignalR;
using LibraryAbstractDBProvider;
using Serilog;
using Serilog.Filters;
var builder = WebApplication.CreateBuilder(args);
//Apply Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());   
});
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

//AddController
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add memory
builder.Services.AddMemoryCache();
//Add JsonPatch
builder.Services.AddControllers().AddNewtonsoftJson();

//Add AutoMapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Config AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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
if (!GetStringAppsetting.DatabaseDefault().Equals(Convert.ToString(Database.MongoDB)))
{
    var createDB = new AutoCreateDB();
    createDB.CreateDB(app);
}
app.UseCors("CorsPolicy");

//Add middleware 
app.UseMiddleware<JwtMiddleware>();
app.UseResponseCaching();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
//Add SignlR
app.MapHub<BroadcastHub>("/notify");
app.UseHttpLogging();

app.Run();