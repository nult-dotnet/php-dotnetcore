using LibraryAbstractDBProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDesktop.DatabaseFactory
{
    public interface IDBAbstractFactory
    {
        IDatabaseProvider GetDatabase();
    }
}