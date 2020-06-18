using System.Collections.Generic;
using HotelMotel.Business.Models;
using HotelMotel.Business.ViewModels;

namespace HotelMotel.Business.IFacade
{
    public interface IFormB
    {
        List<PurposeAmount> FetchPurposeAmount(string cicoid, string sYear, string atCode);
        string UpdatePurposeAmounts(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel);

        List<DmoResults> FetchDmoResults(string cicoid, string sYear, string atCode);
        int UpdateDmoResults(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel);
        List<DmoResults> AddDmoContract(string cicoid, string sYear, string atCode);

        List<ParksResults> FetchParksResults(string cicoid, string sYear, string atCode);
        int UpdateParksResults(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel);
        List<ParksResults> AddParksContract(string cicoid, string sYear, string atCode);

        int UpdateP2Certification(string cicoid, string fiscalyear, string atCode, bool p2Complete); 
    }
}
