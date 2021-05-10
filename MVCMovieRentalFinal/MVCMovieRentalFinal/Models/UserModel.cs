using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovieRentalFinal.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}