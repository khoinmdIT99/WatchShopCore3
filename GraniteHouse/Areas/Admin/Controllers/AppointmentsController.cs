using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class AppointmentsController : Controller
    {

        private readonly ApplicationDbContext _db;
        private int PageSize = 6;

        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

       
        public async Task<IActionResult> Index(int productPage = 1, string searchName = null, string searchEmail = null, 
            string searchPhone = null, string searchDate = null,string OrderN=null, string OrderS = null, string OrderC = null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Appointments = new List<Models.Appointments>()
            };

            StringBuilder param = new StringBuilder();

            param.Append("/Admin/Appointments?productPage=:");
            param.Append("&OrderN=");
            if (OrderN == "NewOrder")
            {
                param.Append(OrderN);
            }
            param.Append("&OrderS=");
            if (OrderS == "ShippingOrder")
            {
                param.Append(OrderS);
            }
            param.Append("&OrderC=");
            if (OrderC == "CompletedOrder")
            {
                param.Append(OrderC);
            }
            param.Append("&searchName=");
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchDate=");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }




            appointmentVM.Appointments = _db.Appointments.Include(a => a.SalesPerson).OrderBy(a=>a.AppointmentCreatedDate).ToList();
            if (User.IsInRole(SD.AdminEndUser))
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.SalesPersonId == claim.Value).OrderBy(a=>a.AppointmentCreatedDate).ToList();
            }

            if(OrderN == "NewOrder")
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.isConfirmed == false && a.isCompleted == false).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (OrderS == "ShippingOrder")
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.isConfirmed == true && a.isCompleted == false).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (OrderC == "CompletedOrder")
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.isConfirmed == true && a.isCompleted == true).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (searchName != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (searchEmail != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (searchPhone != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerPhoneNumber.ToLower().Contains(searchPhone.ToLower())).OrderBy(a => a.AppointmentCreatedDate).ToList();
            }
            if (searchDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchDate);
                    appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.AppointmentDate.ToShortDateString().Equals(appDate.ToShortDateString())).OrderBy(a => a.AppointmentCreatedDate).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            var count = appointmentVM.Appointments.Count;

            appointmentVM.Appointments = appointmentVM.Appointments.OrderBy(p => p.AppointmentCreatedDate)
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

        //GET Edit
        public async Task<IActionResult> Edit(int? id)
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
                Products=productList2.ToList()
            };

            return View(objAppointmentVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentDetailsViewModel objAppointmentVM,string strUrl)
        {
            if (ModelState.IsValid)
            {


                var appointmentFromDb = _db.Appointments.Where(a => a.Id == objAppointmentVM.Appointment.Id).FirstOrDefault();

                appointmentFromDb.CustomerName = objAppointmentVM.Appointment.CustomerName;
                appointmentFromDb.CustomerEmail = objAppointmentVM.Appointment.CustomerEmail;
                appointmentFromDb.CustomerAddress = objAppointmentVM.Appointment.CustomerAddress;
                appointmentFromDb.AppointmentDate = objAppointmentVM.Appointment.AppointmentDate;
                appointmentFromDb.isConfirmed = objAppointmentVM.Appointment.isConfirmed;
                appointmentFromDb.isCompleted = objAppointmentVM.Appointment.isCompleted;
                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    appointmentFromDb.SalesPersonId = objAppointmentVM.Appointment.SalesPersonId;
                }
                else
                {
                    appointmentFromDb.SalesPersonId = null;
                }
                _db.SaveChanges();

                return Redirect(strUrl);


            }

            return View(objAppointmentVM);
        }


        //GET Detials
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
                Products=productList2.ToList()
            };

            return View(objAppointmentVM);

        }


        //GET Delete
        public async Task<IActionResult> Delete(int? id)
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
                Products=productList2.ToList()
            };

            return View(objAppointmentVM);

        }


        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            _db.Appointments.Remove(appointment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Invoice(int id)
        {
            var productList = (IEnumerable<ProductsSelectedForAppointment>)(from p in _db.Appointments
                                                      join a in _db.ProductsSelectedForAppointment
                                                      on p.Id equals a.AppointmentId
                                                      where a.AppointmentId == id
                                                      select a);
            AppointmentDetailsViewModel objAppointmentVM = new AppointmentDetailsViewModel()
            {
                Appointment = _db.Appointments.Where(a=>a.Id==id).FirstOrDefault(),
                ProductsSelectedForAppointments = productList.ToList()
                               
            };
            //var model = _db.Appointments.SingleOrDefault(i=>i.Id.Equals(id));
            return View(objAppointmentVM);

        }
    }
}