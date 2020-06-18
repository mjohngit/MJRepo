using System.ComponentModel.DataAnnotations;

namespace HotelMotel.Business.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectStatus { get; set; }
        public int ProjectUpdType { get; set; }
    }
}
