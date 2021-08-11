using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class ReviewRepository : ReviewRepositoryBase
    {
        public ReviewRepository(DbContext db) : base(db)
        {

        }
    }
}
