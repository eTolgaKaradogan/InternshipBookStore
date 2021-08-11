using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using _02_Entities.Entities;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _05_MvcWebUI.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly INotificationService _notificationService;

        public CartController(IBookService bookService, INotificationService notificationService)
        {
            _bookService = bookService;
            _notificationService = notificationService;
        }

        public IActionResult AddToCart(int? bookId)
        {
            if (bookId == null)
                return View("NotFound");
            var book = _bookService.Query().SingleOrDefault(p => p.Id == bookId.Value);
            string userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            List<CartModel> cart = new List<CartModel>();
            CartModel cartItem;
            string cartJson;
            if (HttpContext.Session.GetString("cart") != null)
            {
                cartJson = HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<List<CartModel>>(cartJson);
            }
            cartItem = new CartModel()
            {
                BookId = bookId.Value,
                UserId = Convert.ToInt32(userId),
                BookName = book.Name,
                UnitPrice = book.UnitPrice
            };
            cart.Add(cartItem);
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartJson);
            Notify("{0} is added to cart!", cartItem.BookName);
            return RedirectToAction("Index", "Book");
        }

        public IActionResult Index()
        {
            List<CartModel> cart = new List<CartModel>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
            }
            List<CartGroupByModel> cartGroupBy = cart.GroupBy(
                c => new { c.BookId, c.UserId, c.BookName }
                ).Select(cGroupBy => new CartGroupByModel()
                {
                    BookId = cGroupBy.Key.BookId,
                    UserId = cGroupBy.Key.UserId,
                    BookName = cGroupBy.Key.BookName,
                    TotalUnitPriceText = "£" + cGroupBy.Sum(cgb => cgb.UnitPrice).ToString(new CultureInfo("en")),
                    TotalCount = cGroupBy.Count()
                }).ToList();
            cartGroupBy = cartGroupBy.OrderBy(cgb => cgb.BookName).ToList();

            return View(cartGroupBy);
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("cart");
            Notify("Cart is cleared!");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int? bookId, int? userId)
        {
            if (bookId == null || userId == null)
            {
                return View("NotFound");
            }
            CartModel item = null;
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<CartModel> cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
                item = cart.FirstOrDefault(c => c.BookId == bookId.Value && c.UserId == userId.Value);
                if (item != null)
                    cart.Remove(item);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            if (item != null)
                Notify("{0} is removed from cart!", item.BookName);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(ShippingDetails shippingDetails)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string username = User.Identity.Name;

            var notification = new Notification()
            {
                Text = $"The {username} is crated an order."
            };
            _notificationService.Create(notification, username);

            HttpContext.Session.Remove("cart");
            Notify("Thank you {0}, your order is in proccess.");
            return RedirectToAction("Index", "Book");
        }
    }
}
