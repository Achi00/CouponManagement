using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Domain.Entity
{
    public class SystemSetting
    {
        public Guid Id { get; set; }
        public int ReservationDurationMinutes { get; set; }
        public int MerchantEditPeriodHours { get; set; }
    }
}
