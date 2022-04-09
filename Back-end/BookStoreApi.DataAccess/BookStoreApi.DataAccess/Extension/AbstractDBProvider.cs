using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider
{
    public abstract class AbstractDBProvider : IDatabaseProvider
    {
        public abstract void ConnectedDatabase(DbContextOptionsBuilder optionsBuilder);

        public abstract string ConnectionString();
    }
}