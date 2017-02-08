using aspCart.Core.Domain.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Models.Sale
{
    public class OrderManageModel
    {
        public OrderManageModel()
        {
            ActiveTab = "info";
        }

        public string ActiveTab { get; set; }

        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid UserId { get; set; }
        public Guid BillingAddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [Display(Name = "State/Province")]
        public string StateProvince { get; set; }

        [Display(Name = "Zip/Postal Code")]
        public string ZipPostalCode { get; set; }

        public string Country { get; set; }
        public string Telephone { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime OrderPlacementDateTime { get; set; }
        public DateTime? OrderCompletedDateTime { get; set; }

        [Display(Name = "Order status")]
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
