using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class DashboardResults
    {
        public string CurrYear { get; set; }
        public string PrevYear { get; set; }
        public string PrevYear2 { get; set; }
        public bool CurrentYearCompleted { get; set; }
        public bool PreviousYearCompleted{ get; set; }
        public bool PreviousYearCompleted2 { get; set; }
        public bool CurrentYearRequired { get; set; }
        public bool PreviousYearRequired { get; set; }
        public bool YearBeforePreviousYearRequired { get; set; }
        public bool EligibleToComplete { get; set; }
        public bool EntityHasOrdinance { get; set; }
    }
}
