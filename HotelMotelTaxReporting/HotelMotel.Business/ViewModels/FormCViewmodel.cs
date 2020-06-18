using System.Collections.Generic;
using System.Web.Mvc;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.ViewModels
{
    public class FormCViewmodel
    {
        public FormCViewmodel()
        {
            Pcises = new List<Pcis>();
            Projects = new List<Project>();
		}

        public List<Pcis> Pcises { get; set; }
        public List<Project> Projects { get; set; }
    }
}
