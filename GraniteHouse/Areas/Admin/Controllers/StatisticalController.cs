using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.Models;
using GraniteHouse.Data;
using GraniteHouse.Models.ViewModel;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 10;
        public StatisticalController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.EarningAnnuals = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true&&a.AppointmentCreatedDate.Year==DateTime.Now.Year)
                .Select(a => a.TotalAppointment).Sum();
            ViewBag.EarningMonthly = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Year == DateTime.Now.Year
            &&a.AppointmentCreatedDate.Month==DateTime.Now.Month)
                .Select(a => a.TotalAppointment).Sum();
            ViewBag.NewOrders = _db.Appointments.Where(a => a.isCompleted == false && a.isConfirmed == false).Count();
            ViewBag.NewCus = _db.Customers.Where(a=>a.CusPhone!=null).Select(a=>a.CusPhone).Count();
            ViewBag.ProductOutOfStock = _db.Products.Where(a => a.Available == false).Count();
            return View();
        }

        public IActionResult EaringYear()
        {

            ViewBag.EarningThisYear = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Year == DateTime.Now.Year)
                .Select(a => a.TotalAppointment).Sum();
            ViewBag.January = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 1 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
                Select(a => a.TotalAppointment).Sum();
            ViewBag.February = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 2 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.March = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 3 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.April = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 4 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.May = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 5 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.June = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 6 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.July = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 7 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.August = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 8 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.September = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 9 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.October = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 10 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.November = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 11 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            ViewBag.December = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == 12 && a.AppointmentCreatedDate.Year == DateTime.Now.Year).
               Select(a => a.TotalAppointment).Sum();
            return View();
        }

        public async Task<IActionResult> EaringMonth(int productPage = 1, string SelectedMonth=null)
        {
            ViewBag.month = SelectedMonth;
            if(SelectedMonth==null)
            {
                ViewBag.EarningThisMonth = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == DateTime.Now.Month && a.AppointmentCreatedDate.Year == DateTime.Now.Year)
               .Select(a => a.TotalAppointment).Sum();
               
            }
            else
            {
                ViewBag.EarningbyMonth = _db.Appointments.Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == Int32.Parse(SelectedMonth) && a.AppointmentCreatedDate.Year == DateTime.Now.Year)
             .Select(a => a.TotalAppointment).Sum();
            }
          
           
            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Appointments = new List<Models.Appointments>()
            };
            StringBuilder param = new StringBuilder();

            param.Append("/Admin/Statistical/EaringMonth?productPage=:");
            param.Append("&SelectedMonth=");
            if (SelectedMonth != null)
            {
                param.Append(SelectedMonth);
            }
           
            appointmentVM.Appointments = _db.Appointments.Include(a => a.SalesPerson).OrderBy(a => a.AppointmentCreatedDate).
                Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == DateTime.Now.Month && a.AppointmentCreatedDate.Year == DateTime.Now.Year).ToList();
            if (SelectedMonth != null)
            {
                appointmentVM.Appointments = _db.Appointments.Include(a => a.SalesPerson).OrderBy(a => a.AppointmentCreatedDate).
                Where(a => a.isCompleted == true && a.isConfirmed == true && a.AppointmentCreatedDate.Month == Int32.Parse(SelectedMonth) && a.AppointmentCreatedDate.Year == DateTime.Now.Year).ToList();
            }
            var count = appointmentVM.Appointments.Count;
            
            appointmentVM.Appointments = appointmentVM.Appointments.OrderBy(a=>a.AppointmentCreatedDate)              
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();


            appointmentVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(appointmentVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList2 = (IEnumerable<Products>)(from p in _db.Products
                                                       join a in _db.ProductsSelectedForAppointment
                                                       on p.Id equals a.ProductId
                                                       where a.AppointmentId == id
                                                       select p).Include("ProductTypes").Include("Brand");

            var productList = (IEnumerable<ProductsSelectedForAppointment>)(from p in _db.Appointments
                                                                            join a in _db.ProductsSelectedForAppointment
                                                                            on p.Id equals a.AppointmentId
                                                                            where a.AppointmentId == id
                                                                            select a);
            AppointmentDetailsViewModel objAppointmentVM = new AppointmentDetailsViewModel()
            {
                Appointment = _db.Appointments.Include(a => a.SalesPerson).Where(a => a.Id == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                ProductsSelectedForAppointments = productList.ToList(),
                Products = productList2.ToList()
            };

            return View(objAppointmentVM);
        }

    }
}