using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class ProductPlatform
    {
        public int ProductPlatformId { get; set; }
        public int? ProductId { get; set; }
        public int? PlatformId { get; set; }

        public virtual Platform? Platform { get; set; }
        public virtual Product? Product { get; set; }
    }
}
