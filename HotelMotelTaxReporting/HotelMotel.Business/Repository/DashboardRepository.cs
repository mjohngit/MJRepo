using System;
using System.Linq;
using System.Runtime.InteropServices;
using AutoMapper;
using HotelMotel.Data;
using HotelMotel.Business.Models;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Common;

namespace HotelMotel.Business.Repository
{
    public class DashboardRepository : IHmDashboard
    {
        private readonly Utils _utils = new Utils();
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();

        public DashboardResults DashboardResults(string cicoid)
        {
            var aList = _utils.GetYearValues();
            var currentYear = aList[0].ToString();

            var dashResults = _hmEntities.CheckCertificationsDashboard(cicoid, Int32.Parse(currentYear));


            var varDash = dashResults.Select(item =>
                new DashboardResults()
                {
                    CurrYear = currentYear,
                    PrevYear = aList[1].ToString(),
                    PrevYear2 = aList[2].ToString(),
                    CurrentYearCompleted = Convert.ToBoolean(item.CurrentYearCompleted),
                    PreviousYearCompleted = Convert.ToBoolean(item.PreviousYearCompleted),
                    PreviousYearCompleted2 = Convert.ToBoolean(item.YearBeforePreviousYearCompleted),
                    CurrentYearRequired = Convert.ToBoolean(item.CurrentYearRequired),
                    PreviousYearRequired = Convert.ToBoolean(item.PreviousYearRequired),
                    YearBeforePreviousYearRequired = Convert.ToBoolean(item.YearBeforePreviousYearRequired),
                    EligibleToComplete = Convert.ToBoolean(item.EligibleToComplete),
                    EntityHasOrdinance = Convert.ToBoolean(item.EntityHasOrdinance)
                }).SingleOrDefault();

            //var varDash = dashResults.Select(Mapper.DynamicMap<DashboardResults>).SingleOrDefault();
            return varDash;
        }
    }
}
