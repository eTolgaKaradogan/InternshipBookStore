using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class CategoryRepository : CategoryRepositoryBase
    {
        public CategoryRepository(DbContext db) : base(db)
        {

        }
    }
}
