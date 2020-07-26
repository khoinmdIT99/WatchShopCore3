using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Models;
namespace GraniteHouse.interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Appointments appointments);
    }
}
