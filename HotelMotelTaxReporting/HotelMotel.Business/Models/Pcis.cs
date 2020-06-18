using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class Pcis
    {
        
        public int PcisId { get; set; }
        //[Required(ErrorMessage = "Please enter Entity Name", AllowEmptyStrings = false)]
        public string Entity { get; set; }
        //[Required(ErrorMessage = "Please enter Project Name", AllowEmptyStrings = false)]
        public string EntityProject { get; set; }
        //[Required(ErrorMessage = "Please enter Promotion Itmems", AllowEmptyStrings = false)]
        public string PromotionItems { get; set; }

        [Required(ErrorMessage = "Enter 0 or more")]
        [Range(int.MinValue, 10000000, ErrorMessage = "Amount too big")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public int ExpenditureAmount { get; set; }
        public int PcisUpdType { get; set; }
    }
}
