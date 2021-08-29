using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectWeb.Models;

namespace ProjectWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly Web1209Context _db;
        public ProductController(Web1209Context db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Product> lstProduct = _db.Products.ToList();
            return View(lstProduct);
        }

        public IActionResult Details(int id)
        {
            Product p = _db.Products.Find(id);
            return View(p);
        }

        public IActionResult addCart(int id)
        {
            ProductManager func = new ProductManager(_db);
            var cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                Product product = func.getDetailsProduct(id);
                List<CartItem> listCart = new List<CartItem>()
               {
                   new CartItem
                   {
                       Product = product,
                       Quantity = 1
                   }
               };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
                return RedirectToAction("ListCart", "Product");
            }
            else
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Product.Id == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                }
                if (check)
                {
                    dataCart.Add(new CartItem
                    {
                        Product = func.getDetailsProduct(id),
                        Quantity = 1
                    });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));               
            }
            return RedirectToAction("ListCart", "Product");
        }

        public IActionResult ListCart()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult updateCart(IFormCollection form)
        {
            int id = int.Parse(form["IdProduct"]);
            int quantity = int.Parse(form["Quantity"]);
            int stock = int.Parse(form["stock"]);
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (quantity > 0)
                {
                    if(quantity > stock)
                    {                       
                        HttpContext.Session.SetString("Stock", "Not Enough In Stock");
                        return RedirectToAction(nameof(ListCart));
                    }
                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].Product.Id == id)
                        {
                            dataCart[i].Quantity = quantity;
                            HttpContext.Session.SetString("Stock","");
                        }
                    }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));                   
                }
            }
            return RedirectToAction(nameof(ListCart));
        }

        public IActionResult deleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Product.Id   == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return RedirectToAction(nameof(ListCart));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Receipt() {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
            }
            return View();
        }

        public IActionResult SearchProduct(string SearchProduct)
        {
            ViewData["GetProductDetails"] = SearchProduct;
            List<Product> products = _db.Products.ToList();
            if (!String.IsNullOrEmpty(SearchProduct))
            {
                products = _db.Products.Where(m => m.Name.Contains(SearchProduct)).ToList();
            }
            return View(products);
        }
    }
}
