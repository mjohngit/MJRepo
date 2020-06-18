using System.Collections.Generic;
using HotelMotel.Business.Models;
using HotelMotel.Business.ViewModels;

namespace HotelMotel.Business.IFacade
{
    public interface ICert
    {
        Certification FetchCertification(string cicoid, string sYear, string atCode);
        int UpdateCertification(string cicoid, string sYear, string atCode, Certification certModel);
        void NotifyPreparerOnSubmit(string cicoid, string sYear, string emailAddress);
    }
}
