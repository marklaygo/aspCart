using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Models.Sale
{
    public class OrderListModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime OrderPlacementDateTime { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }
}
