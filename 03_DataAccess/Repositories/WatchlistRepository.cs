using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class WatchlistRepository : WatchlistRepositoryBase
    {
        public WatchlistRepository(DbContext db) : base(db)
        {

        }
    }
}
