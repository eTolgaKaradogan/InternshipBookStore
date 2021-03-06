using System;
using System.Linq;
using System.Linq.Expressions;
using _01_AppCore.Records.Bases;
using Microsoft.EntityFrameworkCore;

namespace _01_AppCore.Bases.EntityFramework
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : RecordBase, new()
    {
        private readonly DbContext _db;

        protected RepositoryBase(DbContext db)
        {
            _db = db;
        }

        public void Add(TEntity entity, bool save = true)
        {
            try
            {
                _db.Set<TEntity>().Add(entity);
                if (save)
                    Save();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Delete(TEntity entity, bool save = true)
        {
            try
            {
                _db.Set<TEntity>().Remove(entity);
                if (save)
                    Save();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public virtual void DeleteEntity(int id, bool save = true)
        {
            try
            {
                var entity = EntityQuery(e => e.Id == id).SingleOrDefault();
                Delete(entity, save);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IQueryable<TEntity> Query()
        {
            return _db.Set<TEntity>().AsQueryable();
        }

        public virtual IQueryable<TEntity> EntityQuery(params string[] entitiesToInclude)
        {
            var query = Query();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }

        public virtual IQueryable<TEntity> EntityQuery(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            var query = EntityQuery(entitiesToInclude);
            return query.Where(predicate);
        }

        public int Save()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Update(TEntity entity, bool save = true)
        {
            try
            {
                _db.Set<TEntity>().Update(entity);
                if (save)
                    Save();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #region Dispose
        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _db?.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
