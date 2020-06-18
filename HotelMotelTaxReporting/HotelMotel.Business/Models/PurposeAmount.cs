using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class PurposeAmount
    {
        public string PCode { get; set; }
        public string PPurpose { get; set; }
       
        [Required(ErrorMessage = "Enter 0 or more")]
        [Range(int.MinValue, 100000000,ErrorMessage = "Amount too big")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public int PAmount { get; set; }
    }
}
