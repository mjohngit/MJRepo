using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.Repository
{
    public class FormOneRepository : IHmFormOne
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();

        public FormOne FetchFormOneData(string cicoid, string surveyYear)
        {
            var formOneResults = _hmEntities.GetOrdinance(cicoid, Int32.Parse(surveyYear));

            var varResults = formOneResults.Select(item =>
                item.Ordinance_Date != null ? (item.Tax_Rate != null ? new FormOne()
                {
                    Paragraph = item.Paragraph,
                    TaxRate = (decimal) item.Tax_Rate,
                    OrdDate = (DateTime) item.Ordinance_Date,
                    AtCode = item.AtCode,
                    P1Complete = item.P1_Complete,
                    OrdCorrect = item.OrdinanceCorrect,
                    ShortTermRentals = item.ShortTermRentals,
                    CertTableMatchesOrd = item.CertTableMatchesOrdTable,
                    CertRecordExist = item.CertRecordExists
                } : null) : null).FirstOrDefault();
           
            return varResults;
        }

        public int UpdateP1Certification(string cicoid, string sYear, string atCode, bool p1Complete, bool ordCorrect, bool shortTermRentals)
        {
            try
            {
                _hmEntities.pr_Update_P1(cicoid, sYear, atCode, p1Complete, ordCorrect, shortTermRentals);
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int FillPurposeAmounts(string cicoid, string fiscalYr, string atCd)
        {
            try
            {
                _hmEntities.FillPurposeAmts(cicoid, fiscalYr, atCd);
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
