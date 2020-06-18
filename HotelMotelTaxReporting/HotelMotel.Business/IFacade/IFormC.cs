using System.Collections.Generic;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.IFacade
{
    public interface IFormC
    {
        List<Form3> FetchFileDetails(string cicoid, string sYear, string atCd);
        string AddUploadFileDbEntry(string cicoid, string sYear, string atCd, string fileName, string flUrl);
        int DeleteFileNDbrow(int docId);
        bool FindPcisDocNeeded(string cicoid, string sYear);
        int UpdateP3Certification(string cicoid, string fiscalyear, string atCode, bool p3Complete); 
    }
}
