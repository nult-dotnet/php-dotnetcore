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

        public virtual void Commit() {
            Console.WriteLine("Commit");
        }

        public virtual void CreateTransaction() {
            Console.WriteLine("Create Transaction");
        }

        public virtual void Rollback() {
            Console.WriteLine("Rollback");
        }

        public abstract void Save();
    }
}