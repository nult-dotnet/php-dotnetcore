﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider
{
    public interface IDatabaseProvider
    {
        public string ConnectionString();
        public void ConnectedDatabase(DbContextOptionsBuilder optionsBuilder);
    }
}