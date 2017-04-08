using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Models
{
    public class FilterViewModel
    {
        public List<CategoryFilterViewModel> CategoryFilterViewModel { get; set; }

        public IEnumerable<IGrouping<int, decimal>> PriceGroupings { get; set; }

        public int[] PriceRange { get; set; }

        public string FilterType { get; set; }
    }

    public class CategoryFilterViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
