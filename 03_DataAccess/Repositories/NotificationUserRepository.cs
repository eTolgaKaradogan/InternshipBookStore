using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class NotificationUserRepository : NotificationUserRepositoryBase
    {
        public NotificationUserRepository(DbContext db) : base(db)
        {

        }
    }
}
