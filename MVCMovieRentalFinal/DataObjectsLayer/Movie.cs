using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjectsLayer
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public int MovieDate { get; set; }
        public string MovieStatusID { get; set; }
        public bool Active { get; set; }
    }
}
