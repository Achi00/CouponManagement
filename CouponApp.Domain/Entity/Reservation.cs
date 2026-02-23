using CouponApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Domain.Entity
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid OfferId { get; set; }
        public Guid UserId { get; set; }

        public int Quantity { get; set; }

        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public ReservationStatus Status { get; set; }

        public Offer Offer { get; set; }
    }
}
