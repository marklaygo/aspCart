using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Models.Catalog
{
    public class SpecificationCreateOrUpdateModel
    {
        public SpecificationCreateOrUpdateModel()
        {
            Published = true;
            ActiveTab = "info";
        }

        public string ActiveTab { get; set; }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int Position { get; set; }
        public bool Published { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
