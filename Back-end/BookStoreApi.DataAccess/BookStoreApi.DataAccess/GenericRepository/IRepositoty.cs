using LibraryAbstractDBProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.DataAccess.GenericRepository
{
    public interface IRepository<T> : IGenericRepository<T> where T : class
    {
    }
}