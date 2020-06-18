using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class Form3
    {
        public int PcisDocId { get; set; }
        public string PcisCicoid { get; set; }
        public string PcisYear { get; set; }
        public string PcisAtCode { get; set; }
        public string PcisDocName { get; set; }
        public string PcisDocUrl { get; set; }
        public bool PcisDocNeeded { get; set; }
        public string PcisFrmName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PcisDocDt { get; set; }
    }
}
