using System;
using System.Linq;
using System.Net.Mail;
using HotelMotel.Data;
using Ini;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;


namespace HotelMotel.Business.Repository
{
    public class CertificationRepository : ICert
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();
        const string StrMvcfile = "C:\\CMapps\\Config\\mvcmasterkeys.ini";

        public Certification FetchCertification(string cicoid, string sYear, string atCode)
        {
            var varCert = _hmEntities.GetCertification(cicoid, sYear, atCode);

            var cert = varCert.Select(item =>
                new Certification
                {
                    CertComplete = item.Cert_Complete,
                    DateCertified = Convert.ToDateTime(item.Date_Certified),
                    CeoName = item.CEO_Name,
                    CeoTitle = item.CEO_Title,
                    PreparerName = item.Preparer_Name,
                    PreparerTitle = item.Preparer_Title,
                    PreparerPhone = item.Preparer_Phone,
                    PreparerEmail = item.Preparer_Email,
                    PcisProviderName = item.PCIS_Provider_Name,
                    PcisProviderTitle = item.PCIS_Provider_Title,
                    PcisProviderPhone = item.PCIS_Provider_Phone,
                    PcisProviderEmail = item.PCIS_Provider_Email
                }).SingleOrDefault();

            var isFileUploaded = _hmEntities.HMT_PCIS_Docs.Where(
                    x => x.CICOID == cicoid && x.FiscalYear == sYear && x.ATCode == atCode).
                    Select(item =>
                    new Form3()
                    {
                        PcisDocId = item.PCIS_DocId,
                        PcisDocName = item.PCIS_DocName,
                        PcisDocUrl = "../" + item.PCIS_DocUrl + "/" + item.PCIS_DocName,
                        PcisDocDt = item.PCIS_DocDt
                    }).FirstOrDefault();

            if (isFileUploaded != null) cert.Form3DocUploaded = isFileUploaded.PcisDocId != 0;

            if (cert != null && cert.DateCertified.ToString() == "1/1/0001 12:00:00 AM")
                cert.DateCertified = DateTime.Now;

            return cert;
        }

        public int UpdateCertification(string cicoid, string sYear, string atCode, Certification certModel)
        {
            var varCert = certModel;
            int retVal;
            try
            {
                _hmEntities.UpdateCertification(cicoid, sYear, atCode, varCert.CertComplete, varCert.DateCertified,
                    varCert.CeoName, varCert.CeoTitle, varCert.PreparerName, varCert.PreparerTitle,
                    varCert.PreparerPhone, varCert.PreparerEmail, varCert.PcisProviderName, varCert.PcisProviderTitle,
                    varCert.PcisProviderPhone, varCert.PcisProviderEmail);
                retVal = 1;
            }
            catch (Exception)
            {
                throw;
            }
            return retVal;
        }

        public void NotifyPreparerOnSubmit(string cicoid, string sYear, string emailAddress)
        {
            string userName = "dcaapps@dca.ga.gov";
            string password = "$Dc@^A7laNt$";

            var sourceFile = new IniFile(StrMvcfile);
            var body = "<div style='font-family:Arial;font-size: 11pt;'>Thank you for submitting the Report.  Your Hotel-Motel Tax Report for fiscal year " + sYear +
                " has been received. <p>If you have questions, or IF you later need to EDIT your report, please contact <a href='mailto:DCA.Research@dca.ga.gov'>DCA.Research@dca.ga.gov</a>.</p></div>";

            var message = new MailMessage();
            var fromAddress = new MailAddress(userName, "Hotel Motel Tax Reporting Application");
            message.From = fromAddress;
            _ = new MailAddress(emailAddress);
            message.To.Add(emailAddress);
            
            //message.From = new MailAddress("donotreply@dca.ga.gov");
            message.Subject = "Hotel-Motel Tax Report " + sYear;
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = sourceFile.IniReadValue("mvccommon", "dcasmtphost");
                    smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                    smtp.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }
    }
}
