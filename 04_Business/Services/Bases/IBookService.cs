using System;
using System.Collections.Generic;
using _01_AppCore.Business.Models.Ordering;
using _01_AppCore.Business.Models.Paging;
using _01_AppCore.Business.Models.Results;
using _01_AppCore.Business.Services.Bases;
using _04_Business.Models;
using _04_Business.Models.Filters;
using _04_Business.Models.Reports;

namespace _04_Business.Services.Bases
{
    public interface IBookService : IService<BookModel>
    {
        Result<List<BookReportModel>> GetBooksReport(BooksReportFilterModel filter, PageModel page = null, OrderModel order = null);
    }
}
