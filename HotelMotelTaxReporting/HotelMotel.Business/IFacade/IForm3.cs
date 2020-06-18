using System.Collections.Generic;
using HotelMotel.Business.Models;
using HotelMotel.Business.ViewModels;

namespace HotelMotel.Business.IFacade
{
    public interface IForm3
    {
        List<Pcis> FetchPcises(string cicoid, string sYear, string atCode, bool blnPrint);
        int UpdatePcises(string cicoid, string sYear, string atCode, FormCViewmodel formCViewmodel);
        List<Pcis> AddPcises(string cicoid, string sYear, string atCode);

        List<Project> FetchProjects(string cicoid, string sYear, string atCode, bool blnPrint);
        int UpdateProjects(string cicoid, string sYear, string atCode, FormCViewmodel formCViewmodel);
        List<Project> AddProjects(string cicoid, string sYear, string atCode);
        List<ProjectStatus> FetchProjectStatuses(); 

        int UpdateP3Certification(string cicoid, string fiscalyear, string atCode, bool p3Complete); 
    }
}
