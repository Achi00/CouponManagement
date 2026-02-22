using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException(string? message) : base(message)
        {
        }
    }
}
