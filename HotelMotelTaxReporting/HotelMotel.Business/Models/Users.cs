using System.ComponentModel.DataAnnotations;

namespace HotelMotel.Business.Models
{
    public class Users
    {
        [Required(ErrorMessage = "Please enter User Name", AllowEmptyStrings = false)]
        public string LoginId { get; set; }
        [Required(ErrorMessage = "Please enter Password", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
