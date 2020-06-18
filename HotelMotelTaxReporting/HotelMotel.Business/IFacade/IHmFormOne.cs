using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.IFacade
{
    public interface IHmFormOne
    {
        FormOne FetchFormOneData(string cicoid, string sYear);

        int UpdateP1Certification(string cicoid, string sYear, string atCode, bool p1Complete, bool ordCorrect, bool shortTermRentals);

        int FillPurposeAmounts(string cicoid, string sYear, string atCode);
    }
}
