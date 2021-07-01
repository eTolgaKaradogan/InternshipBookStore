using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class UserRepository : UserRepositoryBase
    {
        public UserRepository(DbContext db) : base(db)
        {

        }
    }
}
