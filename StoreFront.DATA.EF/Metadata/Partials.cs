using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    #region Category
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }
    #endregion

    #region Product
    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
    #endregion

    #region Product Platform
    [ModelMetadataType(typeof(ProductPlatformMetadata))]

    public partial class ProductPlatform { }
    #endregion

    #region Platform
    [ModelMetadataType(typeof(PlatformMetadata))]

    public partial class Platform { }
    #endregion

    #region Order
    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class Order { }
    #endregion

    #region Manufacturer
    [ModelMetadataType(typeof(ManufacturerMetadata))]

    public partial class Manufacturer { }
    #endregion

    #region User Info
    [ModelMetadataType(typeof(UserInfoMetadata))]

    public partial class UserInfo { }
    #endregion


}
