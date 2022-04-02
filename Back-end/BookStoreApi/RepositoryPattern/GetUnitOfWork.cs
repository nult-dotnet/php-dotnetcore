using LibraryAbstractDBProvider;
namespace BookStoreApi.RepositoryPattern
{
    public class GetUnitOfWork<TEntity> where TEntity : class
    {
        public static IUnitOfWork<TEntity> UnitOfWork()
        {
            string databaseDefault = GetStringAppsetting.DatabaseDefault();
            try
            {
                if (databaseDefault.Equals("MongoDB"))
                {
                    return new UnitOfWorkMongoDB<TEntity>();
                }
                return new UnitOfWorkSQL<TEntity>();
            }
            catch
            {
                throw new NotImplementedException("Database deafault not a valid database type");
            }
        }
    }
}