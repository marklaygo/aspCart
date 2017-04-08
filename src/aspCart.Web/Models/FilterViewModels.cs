using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Models
{
    public class FilterViewModel
    {
        public List<CategoryFilterViewModel> CategoryFilterViewModel { get; set; }

        public List<ManufacturerFilterViewModel> ManufacturerFilterViewModel { get; set; }

        public IEnumerable<IGrouping<int, decimal>> PriceGroupings { get; set; }

        public List<int> PriceRange { get; set; }

        public string FilterType { get; set; }
    }

    public class CategoryFilterViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class ManufacturerFilterViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
