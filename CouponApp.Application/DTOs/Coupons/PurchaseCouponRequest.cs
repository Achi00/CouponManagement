using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.DTOs.Coupons
{
    // for api endpoint
    public class PurchaseCouponRequest
    {
        public Guid OfferId { get; set; }
    }
}
