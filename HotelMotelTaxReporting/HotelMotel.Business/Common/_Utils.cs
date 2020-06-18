using System.Collections;
using Ini;

namespace HotelMotel.Business.Common
{
    public class Utils
    {
        const string StrMvcfile = "C:\\CMapps\\Config\\mvcmasterkeys.ini";

        public ArrayList GetYearValues()
        {
            var sourceFile = new IniFile(StrMvcfile);
            var currYear = sourceFile.IniReadValue("mvccommon", "currentyear");
            var prevYear = sourceFile.IniReadValue("mvccommon", "previousyear");
            var prevYear2 = sourceFile.IniReadValue("mvccommon", "previousyear2");

            var aList = new ArrayList();
            aList.Add(currYear);
            aList.Add(prevYear);
            aList.Add(prevYear2);

            return aList;
        }
    }
}
