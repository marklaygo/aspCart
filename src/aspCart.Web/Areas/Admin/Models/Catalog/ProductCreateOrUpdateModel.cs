using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Models.Catalog
{
    public class ProductCreateOrUpdateModel
    {
        public ProductCreateOrUpdateModel()
        {
            Published = true;
            ActiveTab = "info";

            StockQuantity = 1000;
            MinimumStockQuantity = 0;
            NotifyForQuantityBelow = 1;
            MinimumCartQuantity = 1;
            MaximumCartQuantity = StockQuantity;
        }

        public string ActiveTab { get; set; }

        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000)]
        [Display(Name = "Product Price")]
        public decimal? Price { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Special Product Price")]
        public decimal? SpecialPrice { get; set; }

        [Display(Name = "Special Price Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? SpecialPriceStartDate { get; set; }

        [Display(Name = "Special Price EndDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? SpecialPriceEndDate { get; set; }

        [Display(Name = "Stock Quantity")]
        public int? StockQuantity { get; set; }

        [Display(Name = "Minimum Stock Qty")]
        public int? MinimumStockQuantity { get; set; }

        [Display(Name = "Notify for quantity below")]
        public int? NotifyForQuantityBelow { get; set; }

        [Display(Name = "Display Availability")]
        public bool DisplayAvailability { get; set; }

        [Display(Name = "Minimum Cart Qty")]
        public int? MinimumCartQuantity { get; set; }

        [Display(Name = "Maximum Cart Qty")]
        public int? MaximumCartQuantity { get; set; }

        [Display(Name = "SEO Url")]
        [RegularExpression(@"^[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*$", ErrorMessage = "URL must only contains alphanumeric [a-z A-Z 0-9] and dash [-] e.g. abc-123-D45")]
        public string SeoUrl { get; set; }

        [Display(Name = "Meta Tag Title")]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Tag Keywords")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Meta Tag Description")]
        public string MetaDescription { get; set; }

        [Display(Name = "Category")]
        public List<string> CategoryIds { get; set; }

        public SelectList CategorySelectList { get; set; }

        public List<ImageModel> Images { get; set; }

        public List<string> ImageIds { get; set; }

        public List<string> ImageSortOrder { get; set; }

        [Display(Name = "Manufacturer")]
        public List<string> ManufacturerIds { get; set; }

        public SelectList ManufacturerSelectList { get; set; }

        public List<ProductSpecificationModel> Specifications { get; set; }

        public SelectList SpecificationKeySelectList { get; set; }

        public bool Published { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
