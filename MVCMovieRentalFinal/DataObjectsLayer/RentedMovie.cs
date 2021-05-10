using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjectsLayer
{
    public class RentedMovie
    {
        public string MovieTitle { get; set; }
        public string FirstName { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }
    }
}
