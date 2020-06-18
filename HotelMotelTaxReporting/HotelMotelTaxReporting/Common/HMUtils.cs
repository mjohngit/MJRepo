using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using Ini;
using Microsoft.Ajax.Utilities;

namespace HotelMotelTaxReporting.Common
{
    public class HmUtils
    {
        public void CreateLiveSession(string loginId, string cityCountyFlg, string cityName, string cityFips)
        {
            HttpContext.Current.Session["LoginId"] = loginId;
            HttpContext.Current.Session["CityOrCounty"] = cityCountyFlg;
            HttpContext.Current.Session["LoggedCityName"] = cityName;
            HttpContext.Current.Session["LoggedCityFips"] = cityFips;

            FormsAuthentication.SetAuthCookie(loginId, true);
            var user = HttpContext.Current.User;
       
            var isauthenticated = user.Identity.IsAuthenticated;
        }

        public void AbandonSession(string loginId)
        {
            HttpContext.Current.Session["LoginId"] = null;
            HttpContext.Current.Session["CityOrCounty"] = null;
            HttpContext.Current.Session["LoggedCityName"] = null;
            HttpContext.Current.Session["LoggedCityFips"] = null;
            HttpContext.Current.Session["TaxYear"] = null;
            HttpContext.Current.Session["AtCode"] = null;
            //HttpContext.Current.Session["DmoAdd"] = null;
            //HttpContext.Current.Session["ParksAdd"] = null;
            HttpContext.Current.Session["OrdinanceCorrect"] = null;
            HttpContext.Current.Session["P2Complete"] = null;
            HttpContext.Current.Session["P3Complete"] = null;
            HttpContext.Current.Session["CertificationComplete"] = null;


            FormsAuthentication.SignOut();
            IPrincipal user = HttpContext.Current.User;
            var isauthenticated = user.Identity.IsAuthenticated;
        }
            
        public bool IsAuthorized()
        {
            return HttpContext.Current.Session["LoginId"] != null;
        }

        public bool IsAuthorisedForView()
        {
            return Convert.ToBoolean(HttpContext.Current.Session["OrdinanceCorrect"]);
        }

        
        public bool IsAuthorizedForCertification()
        {
            if (HttpContext.Current != null)
               return Convert.ToBoolean(value: HttpContext.Current.Session["P2Complete"]) && Convert.ToBoolean(HttpContext.Current.Session["P3Complete"]);
            else
            {
                return false;
            }
        }


        public void SetMMSession(string sYear)
        {
            HttpContext.Current.Session["TaxYear"] = sYear;
        }

        public bool GetMmSession()
        {
            return HttpContext.Current.Session["TaxYear"] != null;
        }

        public string GetTaxyear()
        {
            return HttpContext.Current.Session["TaxYear"].ToString();
        }
    }
}