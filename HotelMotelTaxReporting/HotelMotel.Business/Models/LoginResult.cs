using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelMotel.Business.Models
{
    public class LoginResult
    {
        public int CicoId { get; set; }
        public string EntityName { get; set; }
        public string AnnexLoginResult { get; set; }
        public string AnnexLoginFips { get; set; }
        public byte Cico { get; set; }
    }
}
