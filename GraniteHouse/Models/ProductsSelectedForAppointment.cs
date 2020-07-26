using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models
{
    public class ProductsSelectedForAppointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointments Appointments { get; set; }

        [Key]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

        public string productName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal price { get; set; }

        public int quantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalQuantity { get; set; }
    }
}
