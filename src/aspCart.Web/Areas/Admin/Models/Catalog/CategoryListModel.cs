using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Models.Catalog
{
    public class CategoryListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameWithParent { get; set; }
        public bool Published { get; set; }
    }
}
