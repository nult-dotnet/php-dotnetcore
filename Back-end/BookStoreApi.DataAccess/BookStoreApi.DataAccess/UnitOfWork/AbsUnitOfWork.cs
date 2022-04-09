using BookStoreApi.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.DataAccess.UnitOfWork
{
    public abstract class AbsUnitOfWork : IUnitOfWork
    {
        public abstract object Context { get; }

        public abstract void Commit();

        public abstract void CreateTransaction();

        public abstract void Rollback();

        public abstract void Save();
    }
}