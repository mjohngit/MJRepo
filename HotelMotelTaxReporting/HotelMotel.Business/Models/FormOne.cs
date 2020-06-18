using System;
using System.ComponentModel.DataAnnotations;

namespace HotelMotel.Business.Models
{
    public  class FormOne
    {
        public string Paragraph { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxRateDisplay { get; set; }
        public string AtCode { get; set; }
        public bool P1Complete { get; set; }
        public bool? OrdCorrect { get; set; }
        public bool? CertTableMatchesOrd { get; set; }
        public bool? CertRecordExist { get; set; }
        public bool? ShortTermRentals { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime OrdDate  { get; set; }
    }
}
