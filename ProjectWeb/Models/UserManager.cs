using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models
{
    public class UserManager
    {
        private readonly Web1209Context _db;
        
        public UserManager(Web1209Context db)
        {
            _db = db;
        }

        public bool checkUserName(string username)
        {
            List<User> user = _db.Users.Where(x => x.UserName.Equals(username)).ToList();
            if(user.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkEmail(string email)
        {
            List<User> user = _db.Users.Where(x => x.Email.Equals(email)).ToList();
            if (user.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string encryptPassword(string password)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding uTF8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(uTF8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }

        public Profile getProfile(int ID)
        {
            Profile profile = _db.Profiles.FirstOrDefault(m=>m.IdUser.Equals(ID));
            return profile;
        }

        public List<Receipt> getOrderFromUser(int userID)
        {
            List<Receipt> receipts = _db.Receipts.Where(m => m.IdUser.Equals(userID)).ToList();
            if(receipts == null)
            {
                return null;
            }
            else
            {               
                return receipts;
            }           
        }

        //public List<Product> getDetailsReceipt(int receiptID)
        //{
        //    List<Product> products = new List<Product>();
        //    List<DetailsReceipt> detailsReceipts = _db.DetailsReceipts.Where(m => m.IdReceipt.Equals(receiptID)).ToList();
        //    foreach(var item in detailsReceipts)
        //    {
        //        Product product = _db.Products.FirstOrDefault(m => m.Id.Equals(item.IdProduct));
        //        products.Add(product);
        //    }
        //    return products;
        //}

        public Receipt getDetailsReceipt(int receiptID)
        {
            Receipt receipt = _db.Receipts.Find(receiptID);
            receipt.DetailsReceipts = _db.DetailsReceipts.Where(m => m.IdReceipt.Equals(receiptID)).ToList();
            foreach(var item in receipt.DetailsReceipts)
            {
                item.IdProductNavigation = _db.Products.Find(item.IdProduct);
            }
            return receipt;
        }
    }
}
