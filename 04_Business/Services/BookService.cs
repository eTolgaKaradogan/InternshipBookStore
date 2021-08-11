using System;
using System.Globalization;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class BookService : IBookService
    {
        private readonly BookRepositoryBase _bookRepository;

        public BookService(BookRepositoryBase bookRepository)
        {
            _bookRepository = bookRepository;
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

                    StockAmount = model.StockAmount,

                    UnitPrice = model.UnitPrice,

                    Reviews = model.Reviews,

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

        public IQueryable<BookModel> Query()
        {
            try
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
                    Reviews = p.Reviews,
                    IsEnabled = p.IsEnabled,
                    AuthorName = p.AuthorName,
                    Category = new CategoryModel()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },

                    ImageFileName = p.ImageFileName
                });
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                var entity = _bookRepository.EntityQuery(b => b.Id == model.Id).SingleOrDefault();
                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description?.Trim();
                entity.Name = model.Name.Trim();
                entity.StockAmount = model.StockAmount;
                entity.UnitPrice = model.UnitPrice;
                entity.AuthorName = model.AuthorName;
                entity.IsEnabled = model.IsEnabled;
                entity.Isbn = model.Isbn;

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
