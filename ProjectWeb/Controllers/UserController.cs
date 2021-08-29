using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly Web1209Context _db;

        private readonly IWebHostEnvironment WebHostEnvironment;

        public UserController(Web1209Context db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            WebHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        private string UploadFile(IFormFile Images)
        {
            string fileName = null;
            if (Images != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "avatar");
                fileName = Guid.NewGuid().ToString() + "-" + Images.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Images.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        [HttpPost]
        public IActionResult Login(Login L,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager(_db);
                User user = _db.Users.FirstOrDefault(x => x.UserName.Equals(L.UserName) && x.Password.Equals(UM.encryptPassword(L.Password)));
                if (user != null)
                {
                    HttpContext.Session.SetString("Username", L.UserName);
                    HttpContext.Session.SetInt32("ID", user.Id);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Login", "Đăng nhập thất bại");
                }
            }           
            return View();
            
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register R)
        {
            UserManager UM = new UserManager(_db);
            if (ModelState.IsValid)
            {
                if (UM.checkUserName(R.UserName) && UM.checkEmail(R.Email))
                {
                    User u = new User();                  
                    u.Email = R.Email;
                    u.Role = "User";
                    u.UserName = R.UserName;
                    u.Password = UM.encryptPassword(R.Password);
                    u.Status = true;
                    u.Datejoin = DateTime.Now.ToString();
                    _db.Users.Add(u);
                    _db.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ModelState.AddModelError("Register", "Trùng tên tài khoản hoặc email !");                   
                }
            }
     
            return View();
        }

        public IActionResult Profile(int ID)
        {
            UserManager func = new UserManager(_db);
            Profile profile = func.getProfile(ID);
            if(profile != null)
            {
                return RedirectToAction("ShowProfile","User",new { ID = ID });
            }
            else
            {
                return RedirectToAction("CreateProfile", "User", new { UserID = ID });
            }
        }

        public IActionResult CreateProfile()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult CreateProfile(Profile model, int UserID,IFormFile Images)
        {
            if (ModelState.IsValid)
            {
                Profile profile = new Profile();
                profile.FullName = model.FullName;
                profile.Address = model.Address;
                profile.Phone = model.Phone;
                profile.Birthday = model.Birthday;
                profile.Gender = model.Gender;
                profile.IdUser = UserID;
                string fileName = UploadFile(Images);
                if (fileName == null)
                {
                    fileName = "default.jpg";
                }
                profile.Avatar = fileName;
                _db.Profiles.Add(profile);
                _db.SaveChanges();
                return RedirectToAction("ShowProfile","User",new {ID = profile.IdUser});
            }
            else
            {
                ModelState.AddModelError("CreateFailed", "Cập nhật profile thất bại !");
            }
            return View();
        }

        public IActionResult ShowProfile(int ID)
        {
            Profile profile = _db.Profiles.FirstOrDefault(m => m.IdUser.Equals(ID));
            User user = _db.Users.Find(ID);
            profile.IdUserNavigation = user;
            return View(profile);
        }

        public IActionResult EditProfile(int ID)
        {
            Profile profile = _db.Profiles.FirstOrDefault(m => m.IdUser.Equals(ID));
            User user = _db.Users.Find(ID);
            profile.IdUserNavigation = user;
            return View(profile);
        }

        [HttpPost]
        public IActionResult EditProfile(Profile model, IFormFile Images)
        {
            Profile profile = _db.Profiles.FirstOrDefault(m => m.IdUser.Equals(model.IdUser));
            profile.FullName = model.FullName;
            profile.Address = model.Address;
            model.Phone = model.Phone;
            string fileName = UploadFile(Images);
            if (fileName != null)
            {
                profile.Avatar = fileName;
            }
            _db.SaveChanges();
            return RedirectToAction("ShowProfile", "User", new { ID = profile.IdUser });
        }

        [HttpPost]
        public IActionResult CreateReceipt()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<CartItem> dataCart = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (dataCart.Count > 0)
                {
                    Receipt receipt = new Receipt() { IdUser = HttpContext.Session.GetInt32("ID") , Date = DateTime.Now };
                    _db.Receipts.Add(receipt);
                    _db.SaveChanges();
                    foreach (var item in dataCart)
                    {
                        Product product = _db.Products.Find(item.Product.Id);
                        DetailsReceipt details = new DetailsReceipt()
                        {
                            IdProduct = product.Id,
                            IdReceipt = receipt.Id,                          
                            Quantity = item.Quantity
                        };
                        _db.DetailsReceipts.Add(details);
                        product.Stock -= item.Quantity;
                    }
                    _db.SaveChanges();
                    HttpContext.Session.Remove("cart");
                }
            }
            return RedirectToAction("Index", "Product");
        }

        public IActionResult HistoryOrder(int ID)
        {
            UserManager func = new UserManager(_db);
            List<Receipt> receipts = func.getOrderFromUser(ID);     
            return View(receipts);
        }

        public IActionResult DetailsReceipt(int ID)
        {
            UserManager func = new UserManager(_db);
            Receipt detailsReceipt = func.getDetailsReceipt(ID);
            return View(detailsReceipt);
        }

    }
}
