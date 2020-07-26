using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace GraniteHouse.Models
{
    public class Appointments
    {
       
        public int Id { get; set; }

        [Display(Name ="Sales Person")]
        public string SalesPersonId { get; set; }

        [ForeignKey("SalesPersonId")]
        public virtual ApplicationUser SalesPerson { get; set; }
        [Display(Name = "Created Date")]
        public DateTime AppointmentCreatedDate { get; set; }
        [Display(Name = "Delivery Date")]
        public DateTime AppointmentDate { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Phone")]
        public string CustomerPhoneNumber { get; set; }
        [ForeignKey("CustomerPhoneNumber")]
        public virtual Customer Customer { get; set; }
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }
        [Display(Name = "Customer Address")]
        public string CustomerAddress { get; set; }
        [Display(Name = "Confirmed")]
        public bool isConfirmed { get; set; }
        [Display(Name = "Completed")]
        public bool isCompleted { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Total")]
        public decimal TotalAppointment { get; set; }

        public string Note { get; set; }


    }
}
