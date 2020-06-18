using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class Mainmenu
    {
        public DateTime FyEnds { get; set; }
        public string AtCode { get; set; }
        public bool OrdinanceCorrect { get; set; }
        public bool P1Acknowledge { get; set; }
        public bool P1Complete { get; set; }
        public bool P2Complete { get; set; }
        public bool P3Complete { get; set; }
        public bool CertificationComplete { get; set; }
    }
}
