using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
