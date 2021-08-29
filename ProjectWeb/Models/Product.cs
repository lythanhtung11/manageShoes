using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectWeb.Models
{
    public partial class Product
    {
        public Product()
        {
            DetailsReceipts = new HashSet<DetailsReceipt>();
        }

        

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Condition { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; }
        public string Images { get; set; }
        public int? Stock { get; set; }

        public virtual ICollection<DetailsReceipt> DetailsReceipts { get; set; }
    }
}
