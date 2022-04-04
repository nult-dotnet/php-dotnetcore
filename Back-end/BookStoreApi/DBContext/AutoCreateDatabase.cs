namespace BookStoreApi.DBContext
{
    public class AutoCreateDatabase
    {
        public void CreateDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SQLContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}