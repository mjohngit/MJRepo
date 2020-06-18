using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.Repository
{
    public class FormCRepository : IFormC
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();
        public List<Form3> FetchFileDetails(string cicoid, string sYear, string atCd)
        {
            var form3 =                             
                _hmEntities.HMT_PCIS_Docs.Where(
                    x => x.CICOID == cicoid && x.FiscalYear == sYear && x.ATCode == atCd).
                    Select(item =>
                    new Form3()
                    {
                        PcisDocId = item.PCIS_DocId,
                        PcisDocName = item.PCIS_DocName,
                        PcisDocUrl =  "../" + item.PCIS_DocUrl + "/" + item.PCIS_DocName,
                        PcisDocDt = item.PCIS_DocDt
                    }).ToList();
            var num = 0;
            foreach (var itm in form3)
            {
                itm.PcisFrmName = "frmDelete" + num;
                num += 1;
            }
            return form3;
        }

        public string AddUploadFileDbEntry(string cicoid, string sYear, string atCd, string fileName, string flUrl)
        {
            string sMessage;
            
            var form3 = new HMT_PCIS_Docs();
            {
                form3.CICOID = cicoid;
                form3.FiscalYear = sYear;
                form3.ATCode = atCd;
                form3.PCIS_DocName = fileName;
                form3.PCIS_DocUrl = flUrl;
                form3.PCIS_DocDt = DateTime.Now;
            }
            var rowExists =
                _hmEntities.HMT_PCIS_Docs.Where(
                    x => x.CICOID == cicoid && x.FiscalYear == sYear && x.ATCode     == atCd);

            //if (!rowExists.Any())
            if(rowExists.Count () < 5)
            {
                using (var dbCtx = new HotelMotelTaxEntities())
                {
                    dbCtx.HMT_PCIS_Docs.Add(form3);
                    dbCtx.SaveChanges();
                    sMessage = "Successfully added PCIS document";
                }
            }
            else
            {
                using (var dbCtx = new HotelMotelTaxEntities())
                {
                    dbCtx.Entry(rowExists).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.SaveChanges();
                    sMessage = "Successfully updated PCIS document";
                }
            }
            return (sMessage);
        }

        public int DeleteFileNDbrow(int docId)
        {
            int retval;
            try
            {
                //using (var dbCtx = new HotelMotelTaxEntities())
                //{
                //    var docToDelete = _hmEntities.HMT_PCIS_Docs.SingleOrDefault(x => x.PCIS_DocId == form3.PcisDocId); //returns a single item.
                //    if (docToDelete == null) return 0;

                //    dbCtx.HMT_PCIS_Docs.Attach(docToDelete);
                //    HMT_PCIS_Docs hmtPcisDocs = dbCtx.HMT_PCIS_Docs.Remove(docToDelete);
                //    dbCtx.SaveChanges();
                //    retval = 1;
                //}

                var docToDelete = _hmEntities.HMT_PCIS_Docs.SingleOrDefault(x => x.PCIS_DocId == docId); //returns a single item.
                _hmEntities.HMT_PCIS_Docs.Remove(docToDelete);
                _hmEntities.SaveChanges();
                retval = 1;
            }
            catch (Exception ex)
            {
                retval = 0;
            }
            return retval;
        }

        public bool FindPcisDocNeeded(string cicoid, string sYear)
        {
            var formOneResults = _hmEntities.GetOrdinance(cicoid, Int32.Parse(sYear)).FirstOrDefault();
            var blnPcisNeeded = formOneResults == null || formOneResults.PCISRequired;

            return blnPcisNeeded;
        }

        public int UpdateP3Certification(string cicoid, string sYear, string atCode, bool p3Complete)
        {
            int retval;
            try
            {
                _hmEntities.UpdateFormCStatus(atCode, cicoid, sYear, p3Complete);
                retval = 1;
                return retval;
            }
            catch (Exception)
            {
                retval = 0;
                return retval;
            }
        }
    }
}
