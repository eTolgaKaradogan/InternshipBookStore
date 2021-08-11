using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;
using System.Security.Claims;
using System.Net.Http;

namespace _04_Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewRepositoryBase _reviewRepository;
        private readonly BookRepositoryBase _bookRepository;
        private readonly UserRepositoryBase _userRepository;

        public ReviewService(ReviewRepositoryBase reviewRepository, BookRepositoryBase bookRepository, UserRepositoryBase userRepository)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public Result Add(ReviewModel model)
        {
            try
            {
                var entity = new Review()
                {
                    Rating = model.Rating,
                    Content = model.Content,
                    BookId = model.BookId,
                    UserId = model.UserId,
                    Username = model.UserName
                };
                _reviewRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _reviewRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _reviewRepository?.Dispose();
        }

        public IQueryable<ReviewModel> Query()
        {
            var query = _reviewRepository.EntityQuery("Books").OrderBy(r => r.Rating).Select(r => new ReviewModel()
            {
                Rating = r.Rating,
                Content = r.Content,
                UserId = r.UserId,
                BookId = r.BookId,
                UserName = r.Username
            });
            return query;
        }

        public Result Update(ReviewModel model)
        {
            try
            {
                var entity = new Review()
                {
                    Rating = model.Rating,
                    Content = model.Content,
                    UserId = model.UserId,
                    BookId = model.BookId,
                    Username = model.UserName
                };
                _reviewRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
