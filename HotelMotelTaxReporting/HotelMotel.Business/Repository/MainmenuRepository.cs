using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoMapper;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.Repository
{
    public class MainmenuRepository : IHmMenu
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();

        public Mainmenu FetchMainmenu(string cicoid, string surveyYear)
        {
            var mmResults = _hmEntities.HMT_Certifications.Where((o => (o.CICOID == cicoid) &&
                                                                       (o.Fiscal_Year == surveyYear)));
            var varResults = mmResults.Select(item =>
                new Mainmenu()
                {
                    AtCode = item.ATCode,
                    OrdinanceCorrect = item.OrdinanceCorrect,
                    P1Acknowledge = item.P1_Complete,
                    P1Complete = item.P1_Complete,
                    P2Complete = item.P2_Complete,
                    P3Complete = item.P3_Complete,
                    CertificationComplete = item.Cert_Complete
                }).FirstOrDefault();
            //var varResults = mmResults.Select(Mapper.DynamicMap<Mainmenu>).SingleOrDefault();

            return varResults;
        }
    }
}
