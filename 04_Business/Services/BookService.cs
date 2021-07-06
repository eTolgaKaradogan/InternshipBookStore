using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _01_AppCore.Business.Models.Ordering;
using _01_AppCore.Business.Models.Paging;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Models.Filters;
using _04_Business.Models.Reports;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class BookService : IBookService
    {
        private readonly BookRepositoryBase _bookRepository;
        private readonly CategoryRepositoryBase _categoryRepository;

        public BookService(BookRepositoryBase bookRepository, CategoryRepositoryBase categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public Result Add(BookModel model)
        {
            try
            {

                if (_bookRepository.EntityQuery().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Product with the same name exists!");

                decimal unitPrice;

                if (!decimal.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                    return new ErrorResult("Unit price must be a decimal number!");

                model.UnitPrice = unitPrice;
                var entity = new Book()
                {
                    CategoryId = model.CategoryId,

                    Description = model.Description?.Trim(),

                    Name = model.Name.Trim(),

                    AuthorName = model.AuthorName.Trim(),

                    Isbn = model.Isbn,

                    IsEnabled = model.IsEnabled,

                    Rate = model.Rate,

                    StockAmount = model.StockAmount,

                    UnitPrice = model.UnitPrice,

                    ImageFileName = model.ImageFileName
                };
                _bookRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _bookRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _bookRepository?.Dispose();
        }

        public Result<List<BookReportModel>> GetBooksReport(BooksReportFilterModel filter, PageModel page = null, OrderModel order = null)
        {
            try
            {
                #region Query
                var productQuery = _bookRepository.EntityQuery();
                var categoryQuery = _categoryRepository.EntityQuery();

                var query = productQuery.Join(categoryQuery,
                    p => p.CategoryId,
                    c => c.Id,
                    (p, c) => new BookReportModel()
                    {
                        CategoryName = c.Name,
                        BookDescription = p.Description,
                        BookName = p.Name,
                        StockAmount = p.StockAmount,
                        UnitPriceText = "$" + p.UnitPrice.ToString(new CultureInfo("en")),
                        CategoryId = c.Id,
                        UnitPrice = p.UnitPrice,
                        AuthorName = p.AuthorName,
                        Rate = p.Rate,
                        Isbn = p.Isbn,
                        IsEnabled = p.IsEnabled
                    });
                #endregion

                #region Query First Order
                query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.BookName);
                #endregion

                #region Order
                if (order != null && !string.IsNullOrWhiteSpace(order.Expression))
                {
                    switch (order.Expression)
                    {
                        case "Product Name":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.BookName)
                                : query.OrderByDescending(q => q.BookName);
                            break;
                        case "Category Name":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.CategoryName)
                                : query.OrderByDescending(q => q.CategoryName);
                            break;
                        case "Unit Price":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.UnitPrice)
                                : query.OrderByDescending(q => q.UnitPrice);
                            break;
                        case "Stock Amount":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.StockAmount)
                                : query.OrderByDescending(q => q.StockAmount);
                            break;
                        case "Author Name":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.AuthorName)
                                : query.OrderByDescending(q => q.AuthorName);
                            break;
                        case "Isbn":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.Isbn)
                                : query.OrderByDescending(q => q.Isbn);
                            break;
                        case "Is Enabled?":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.IsEnabled)
                                : query.OrderByDescending(q => q.IsEnabled);
                            break;
                        default:
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.Rate)
                                : query.OrderByDescending(q => q.Rate);
                            break;
                    }
                }
                #endregion

                #region Query Filter
                if (filter.CategoryId.HasValue)
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(filter.BookName))
                {
                    query = query.Where(q => q.BookName.ToUpper().Contains(filter.BookName.ToUpper().Trim()));
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceBeginText))
                {
                    decimal unitPriceBegin = Convert.ToDecimal(filter.UnitPriceBeginText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice >= unitPriceBegin);
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceEndText))
                {
                    decimal unitPriceEnd = Convert.ToDecimal(filter.UnitPriceEndText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice <= unitPriceEnd);
                }
                if (filter.StockAmountBegin != null)
                    query = query.Where(q => q.StockAmount >= filter.StockAmountBegin.Value);
                if (filter.StockAmountEnd != null)
                    query = query.Where(q => q.StockAmount <= filter.StockAmountEnd.Value);
                if (!string.IsNullOrEmpty(filter.AuthorName))
                {
                    query = query.Where(q => q.AuthorName.ToUpper().Contains(filter.AuthorName.ToUpper().Trim()));
                }

                if (filter.Isbn.ToString() != null)
                {
                    query = query.Where(q => q.Isbn == filter.Isbn);
                }
                #endregion

                #region Query Paging
                if (page != null)
                {
                    page.RecordsCount = query.Count();
                    int skip = (page.PageNumber - 1) * page.RecordsPerPageCount;
                    int take = page.RecordsPerPageCount;
                    query = query.Skip(skip).Take(take);
                }
                #endregion

                return new SuccessResult<List<BookReportModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<BookReportModel>>(exc);
            }
        }

        public IQueryable<BookModel> Query()
        {
            var query = _bookRepository.EntityQuery("Category").OrderBy(p => p.Name).Select(p => new BookModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                UnitPriceText = p.UnitPrice.ToString(new CultureInfo("en")),
                StockAmount = p.StockAmount,
                CategoryId = p.CategoryId,
                Isbn = p.Isbn,
                IsEnabled = p.IsEnabled,
                AuthorName = p.AuthorName,
                Rate = p.Rate,
                Category = new CategoryModel()
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },

                ImageFileName = p.ImageFileName
            });
            return query;
        }

        public Result Update(BookModel model)
        {
            try
            {
                if (_bookRepository.EntityQuery().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id))
                    return new ErrorResult("Product with the same name exists!");

                decimal unitPrice;
                if (!decimal.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                    return new ErrorResult("Unit price must be a decimal number!");

                model.UnitPrice = unitPrice;
                var entity = _bookRepository.EntityQuery(p => p.Id == model.Id).SingleOrDefault();
                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description?.Trim();
                entity.Name = model.Name.Trim();
                entity.StockAmount = model.StockAmount;
                entity.UnitPrice = model.UnitPrice;
                entity.AuthorName = model.AuthorName;
                entity.IsEnabled = model.IsEnabled;
                entity.Isbn = model.Isbn;
                entity.Rate = model.Rate;

                entity.ImageFileName = model.ImageFileName;

                _bookRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
