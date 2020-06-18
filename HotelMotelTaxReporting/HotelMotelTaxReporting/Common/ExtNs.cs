using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelMotelTaxReporting.Common
{
    public static class Extns
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }

    }
}