using Microsoft.AspNetCore.Mvc;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using StoreFront.UI.MVC.Models;
using Newtonsoft.Json;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Get string
            var sessionCart = HttpContext.Session.GetString("cart");

            //create local cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            var sessionCart = HttpContext.Session.GetString("cart");

            if(String.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            Product product = _context.Products.Find(id);

            var cartItem = new CartItemViewModel(1, product);

            if(shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, cartItem);
            }

            string jsonLocalCart = JsonConvert.SerializeObject(shoppingCart);

            HttpContext.Session.SetString("cart", jsonLocalCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(id);

            if(shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                var jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[productId].Qty = qty;

            var jsonCart = JsonConvert.SerializeObject(shoppingCart);

            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SubmitOrder()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            var userInfo = _context.UserInfos.Find(userId);
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                ShipToName = userInfo.FirstName + " " + userInfo.LastName,
                ShipCity = userInfo.City,
                ShipState = userInfo.State,
                ShipZip = userInfo.Zip
            };

            _context.Orders.Add(order);

            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            foreach (var item in shoppingCart)
            {
                OrderProduct orderProduct = new OrderProduct()
                {
                    OrderId = order.OrderId,
                    ProductId = item.Key,
                    ProductPrice = item.Value.Product.ProductPrice,
                    Quantity = (short)item.Value.Qty
                };

                order.OrderProducts.Add(orderProduct);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Orders");
        }
    }
}
