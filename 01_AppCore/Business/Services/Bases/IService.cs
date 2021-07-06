using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _01_AppCore.Records.Bases;

namespace _01_AppCore.Business.Services.Bases
{
    public interface IService<TModel> : IDisposable where TModel : RecordBase, new()
    {
        IQueryable<TModel> Query();
        Result Add(TModel model);
        Result Update(TModel model);
        Result Delete(int id);
    }
}
