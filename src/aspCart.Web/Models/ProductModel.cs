using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Specifications = new List<SpecificationModel>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string MainImage { get; set; }

        public string SeoUrl { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public List<SpecificationModel> Specifications { get; set; }
    }
}
