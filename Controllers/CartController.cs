using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Needed for .Include()
using pc_mats.Data;
using pc_mats.Models;
using pc_mats.Extensions;

namespace pc_mats.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    // Convert Enum to String here
                    Category = product.Category.ToString(),
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl ?? ""
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult RemoveItem(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null) cart.Remove(item);

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // The URL will now always be: /Cart/Checkout
        public IActionResult Checkout()
        {
            // Simply get whatever is in the cart session (either from "Add to Cart" or "Buy Now")
            var checkoutItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (!checkoutItems.Any())
            {
                return RedirectToAction("Index", "Product");
            }

            return View(checkoutItems);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string paymentMethod, string firstName, string lastName, string address)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any()) return RedirectToAction("Index", "Product");

            // 1. Create the Order object
            var order = new Order
            {
                UserId = HttpContext.Session.GetString("UserName"),
                FullName = firstName + " " + lastName,
                Address = address,
                PaymentMethod = paymentMethod,
                TotalAmount = cart.Sum(x => x.TotalPrice),
                OrderItems = cart.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            // 2. DECREMENT STOCK QUANTITY
            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    // Subtract the quantity sold from the stock
                    product.Quantity -= item.Quantity;

                    // Optional: Prevent negative stock
                    if (product.Quantity < 0) product.Quantity = 0;

                    _context.Products.Update(product);
                }
            }

            // 3. Save everything (Order + Updated Product Quantities)
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 2. Handle the "Empty Cart" logic based on payment type
            if (paymentMethod == "Card")
            {
                return RedirectToAction("PaymentGate");
            }

            // For Cash on Delivery, empty it now
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "COD" });
        }

        // Ensure the Card Payment also empties the cart
        public IActionResult ConfirmCardPayment()
        {
            // The payment went through, so we wipe the session here
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "Card" });
        }

        public IActionResult PaymentGate() => View();

        public IActionResult Success(string method)
        {
            ViewBag.Method = method;
            return View();
        }

        

        public IActionResult MyOrders()
        {
            var userName = HttpContext.Session.GetString("UserName"); // Fixed Context -> HttpContext
            var orders = _context.Orders
                                 .Include(o => o.OrderItems)
                                 .Where(o => o.UserId == userName)
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToList();
            return View(orders);
        }

        public IActionResult AdminOrders()
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ToList();
            return View(orders);
        }
        public IActionResult BuyNow(int id)
        {
            HttpContext.Session.Remove("Cart");

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = new List<CartItem>
    {
        new CartItem
        {
            ProductId = product.Id,
            ProductName = product.ProductName,
            Category = product.Category.ToString(), // Enum to String
            Price = product.Price,
            Quantity = 1,
            ImageUrl = product.ImageUrl ?? ""
        }
    };

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Checkout");
        }

    }
}