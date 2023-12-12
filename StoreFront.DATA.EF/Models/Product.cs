using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            ProductPlatforms = new HashSet<ProductPlatform>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; } = null!;
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? ProductStatus { get; set; }

        public virtual Category? Category { get; set; } 
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ProductStatus? ProductStatusNavigation { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<ProductPlatform> ProductPlatforms { get; set; }
    }
}
