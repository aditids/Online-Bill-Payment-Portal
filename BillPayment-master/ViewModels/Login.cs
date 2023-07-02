using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillPayment.ViewModels
{
    public class Login
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public string Role { get; set; }

        public bool LoggedIn { get; set; }

        public string ErrorMessage { get; set; }

    }
}