using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.Models
{
    public class ProductManager
    {
        private readonly Web1209Context _db;

        public ProductManager(Web1209Context db)
        {
            _db = db;
        }
        public Product getDetailsProduct(int ID)
        {
            Product p = _db.Products.Find(ID);
            return p;
        }

        public List<Product> getAllProduct()
        {
            List<Product> products = _db.Products.ToList();
            return products;
        }
    }
}
