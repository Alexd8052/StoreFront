using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreFront.DATA.EF.Models//.Metadata
{
	#region Category
	public class CategoryMetadata
	{
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "* Category Name is required")]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }
    }
    #endregion

    #region Product
    public class ProductMetadata
    {
        public int ProductId { get; set; }

        [StringLength(200)]
        [Display(Name = "Product")]
        [Required]
        public string ProductName { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        [Display(Name = "Price")]
        [Range(0, (double)decimal.MaxValue)]
        [Required]
        public decimal ProductPrice { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string ProductDescription { get; set; } = null!;

        [StringLength(75)]
        [Display(Name = "Image")]
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? ProductStatus { get; set; }
    } 
    #endregion

    #region Product Platform
    public class ProductPlatformMetadata
    {
        public int ProductPlatformId { get; set; }
        public int? ProductId { get; set; }
        public int? PlatformId { get; set; }
    }
    #endregion

    #region Platform
    public class PlatformMetadata
    {
        [Required]
        public int PlatformId { get; set; }

        [Required]
        [StringLength(50)]
        public string PlatformName { get; set; } = null!;
    }
    #endregion

    #region Order
    public class OrderMetadata
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]//0:d => MM/dd/yyyy
        [Display(Name = "Order Date")]
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Manufacturer")]
        public string ShipToName { get; set; } = null!;

        [Required]
        [Display(Name = "City")]
        [StringLength(50)]
        public string ShipCity { get; set; } = null!;

        [StringLength(2)]
        public string? ShipState { get; set; }

        [Required]
        [StringLength(5)]
        [DataType(DataType.PostalCode)]
        public string ShipZip { get; set; } = null!;

        [Display(Name = "Country")]
        [StringLength(100)]
        public string? ShipCountry { get; set; }
    }
    #endregion

    #region Manufacturer
    public class ManufacturerMetadata
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
    }
    #endregion

    #region User Info
    public class UserInfoMetadata
    {
        public string UserId { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(150)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(5)]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
    }
    #endregion
}
