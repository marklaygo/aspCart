using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Models
{
    public class CartItemModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public int Quantity { get; set; }

        public int MaxCartQuantity { get; set; }

        public string MainImage { get; set; }

        public string SeoUrl { get; set; }
    }
}
