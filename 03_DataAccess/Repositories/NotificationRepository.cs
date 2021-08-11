using System;
using _03_DataAccess.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories
{
    public class NotificationRepository : NotificationRepositoryBase
    {
        public NotificationRepository(DbContext db) : base(db)
        {

        }
    }
}
