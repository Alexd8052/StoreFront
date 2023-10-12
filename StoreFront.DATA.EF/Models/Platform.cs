using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Platform
    {
        public Platform()
        {
            ProductPlatforms = new HashSet<ProductPlatform>();
        }

        public int PlatformId { get; set; }
        public string PlatformName { get; set; } = null!;

        public virtual ICollection<ProductPlatform> ProductPlatforms { get; set; }
    }
}
