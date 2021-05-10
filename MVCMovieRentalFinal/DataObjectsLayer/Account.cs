using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjectsLayer
{
    public class Account
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

        public Account(int userID, string firstName, string lastName, string email, List<string> roles)
        {
            this.UserID = userID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Roles = roles;
        }
    }
}
