using System;
using _01_AppCore.Bases.EntityFramework;
using _02_Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.Repositories.Bases
{
    public class NotificationUserRepositoryBase : RepositoryBase<NotificationUser>
    {
        protected NotificationUserRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
