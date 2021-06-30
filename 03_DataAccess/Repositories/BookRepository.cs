using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class BookRepository : BookRepositoryBase
    {
        public BookRepository(DbContext db) : base(db)
        {

        }
    }
}