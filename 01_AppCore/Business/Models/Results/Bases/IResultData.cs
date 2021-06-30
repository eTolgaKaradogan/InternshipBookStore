using System;
namespace _01_AppCore.Business.Models.Results.Bases
{
    public interface IResultData<out TResultType>
    {
        TResultType Data { get; }
    }
}
