using System;
using _01_AppCore.Bases.EntityFramework;
using _02_Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories.Bases
{
    public abstract class CategoryRepositoryBase : RepositoryBase<Category>
    {
        protected CategoryRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
