using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;
using _05_MvcWebUI.Models;
using _05_MvcWebUI.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _05_MvcWebUI.Controllers
{

    [Authorize]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        private readonly INotificationService _notificationService;
        private readonly UserRepositoryBase _userRepository;
        private readonly NotificationRepositoryBase _notificationRepository;


        public BookController(IBookService bookService, ICategoryService categoryService, IUserService userService, IReviewService reviewService, UserRepositoryBase userRepository, NotificationRepositoryBase notificationRepository, INotificationService notificationService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _userService = userService;
            _reviewService = reviewService;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int? categoryId, int? id, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            var model = _bookService.Query();

            if (User.IsInRole("User"))
            {
                model = _bookService.Query().Where(b => b.IsEnabled);
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = _bookService.Query().Where(b => b.IsEnabled && b.Name.Contains(searchString) || b.AuthorName.Contains(searchString) || b.Isbn.ToString() == searchString);
                }
                if (categoryId.HasValue)
                {
                    model = _bookService.Query().Where(b => b.IsEnabled && b.CategoryId == categoryId);
                }
                ViewBag.BookId = id;
                return View(await PaginatedList<BookModel>.CreateAsync(model, pageNumber, 6));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                model = _bookService.Query().Where(b => b.Name.Contains(searchString) || b.AuthorName.Contains(searchString) || b.Isbn.ToString() == searchString);
            }
            if (categoryId.HasValue)
            {
                model = _bookService.Query().Where(b => b.CategoryId == categoryId);
            }
            ViewBag.BookId = id;
            return View(await PaginatedList<BookModel>.CreateAsync(model, pageNumber, 6));
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            BookDetailsViewModel model = new BookDetailsViewModel()
            {
                bookModel = _bookService.Query().SingleOrDefault(b => b.Id == id)
            };
            if (model == null)
            {
                return View("NotFound");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SendReview(ReviewModel review, double rating, int bookId)
        {
            try
            {
                string userName = User.Identity.Name;
                var user = _userService.Query().FirstOrDefault(u => u.UserName == userName);
                review.UserId = user.Id;
                review.BookId = bookId;
                review.Rating = rating;
                review.UserName = User.Identity.Name;
                _reviewService.Add(review);
                var bookname = _bookService.Query().FirstOrDefault(b => b.Id == bookId).Name;
                var notification = new Notification()
                {
                    Text = $"The {userName} is reviewed the {bookname}."
                };
                _notificationService.Create(notification, review.UserName);
                Notify("Your review is sended.");
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Notify("An error occured!");
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var query = _categoryService.Query();
            ViewBag.Categories = new SelectList(query.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(BookModel book, IFormFile image)
        {
            Result bookResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {
                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false;
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                        return View(book);
                    }

                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                        return View(book);
                    }

                    saveFile = true;
                }
                if (saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension;

                    filePath = Path.Combine("wwwroot", "files", "books", fileName);
                }
                book.ImageFileName = fileName;
                bookResult = _bookService.Add(book);
                if (bookResult.Status == ResultStatus.Exception)
                {
                    Notify("An error occured!");
                    throw new Exception(bookResult.Message);
                }
                if (bookResult.Status == ResultStatus.Success)
                {
                    if (saveFile)
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            image.CopyTo(fileStream);
                        }
                    }
                    Notify($"{book.Name} is successfully created.");
                    return RedirectToAction("AdminIndex");
                }
                ModelState.AddModelError("", bookResult.Message);
                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                return View(book);
            }
            categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("NotFound");
            var book = _bookService.Query().SingleOrDefault(b => b.Id == id.Value);
            if (book == null)
                return View("NotFound");
            var categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(BookModel book, IFormFile image)
        {
            Result bookResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {
                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false;
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                        return View(book);
                    }
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                        return View(book);
                    }
                    saveFile = true;
                }
                var existingBook = _bookService.Query().SingleOrDefault(p => p.Id == book.Id);
                if (string.IsNullOrWhiteSpace(existingBook.ImageFileName))
                {
                    fileName = Guid.NewGuid() + fileExtension;
                }
                else
                {
                    int periodIndex = existingBook.ImageFileName.IndexOf(".");
                    fileName = existingBook.ImageFileName.Substring(0, periodIndex + 1);
                    fileName = fileName + fileExtension;
                }
                book.ImageFileName = fileName;
                bookResult = _bookService.Update(book);
                if (bookResult.Status == ResultStatus.Exception)
                {
                    throw new Exception(bookResult.Message);
                }
                if (bookResult.Status == ResultStatus.Success)
                {
                    if (saveFile)
                    {
                        filePath = Path.Combine("wwwroot", "files", "books", fileName);
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                    }
                    Notify($"{book.Name} is successfully edited.");
                    return RedirectToAction("Details", new { id = book.Id });
                }
                ModelState.AddModelError("", bookResult.Message);
                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
                return View(book);
            }
            categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _bookService.Query().SingleOrDefault(b => b.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var book = _bookService.Query().SingleOrDefault(b => b.Id == id.Value);
                _bookService.Delete(id.Value);
                Notify($"{book.Name} is successfully deleted.");
                return RedirectToAction("AdminIndex");
            }
            catch (Exception ex)
            {
                Notify("An error occured!");
                throw ex;
            }
        }

        public IActionResult DeleteBookImage(int? id)
        {
            if (id == null)
                return View("NotFound");

            var existingBook = _bookService.Query().SingleOrDefault(p => p.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(existingBook.ImageFileName))
            {
                string filePath = Path.Combine("wwwroot", "files", "products", existingBook.ImageFileName);
                existingBook.ImageFileName = null;
                var result = _bookService.Update(existingBook);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            Notify($"{existingBook.Name}'s image is successfully deleted.");
            return RedirectToAction("Details", new { id = id });
        }

        [AllowAnonymous]
        public IActionResult GetByCategory(string message = null, int? categoryId = null)
        {
            var model = _bookService.Query().Where(b => b.CategoryId == categoryId).ToList();
            ViewData["BooksMessage"] = message;
            return View(model);
        }

        public async Task<IActionResult> AdminIndex(string searchString, int pageNumber = 1)
        {
            ViewData["AdminIndexFilter"] = searchString;
            var model = _bookService.Query();
            if (!String.IsNullOrEmpty(searchString))
            {
                model = _bookService.Query().Where(b => b.IsEnabled && b.Name.Contains(searchString) || b.AuthorName.Contains(searchString) || b.Isbn.ToString() == searchString);
            }
            return View(await PaginatedList<BookModel>.CreateAsync(model, pageNumber, 6));
        }
    }
}