using System;
using System.Collections.Generic;
using System.Linq;
using HotelMotel.Business.ViewModels;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.Repository
{
    public class FormBRepository : IFormB
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();

        public List<PurposeAmount> FetchPurposeAmount(string cicoid, string fiscalYr, string atCd)
        {
            var varPurpAmounts = _hmEntities.FetchPurposeAmts(cicoid, fiscalYr, atCd);

            var lstPurposeAmounts = varPurpAmounts.AsEnumerable().Select(item =>
                new PurposeAmount
                {
                    PCode = item.PCode,
                    PPurpose = item.Purpose,
                    PAmount = item.Amount
                }).ToList();
            return lstPurposeAmounts;
        }

        public string UpdatePurposeAmounts(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel)
        {
            var paAmounts = formBViewmodel.PurposeAmountResults;
            var sMessage = "";
            foreach (var pa in paAmounts)
            {
                try
                {
                    _hmEntities.UpdatePurposeAmts(cicoid, sYear, atCode, pa.PCode, pa.PAmount);
                    sMessage = "Purpose Amounts updated successfully";
                }
                catch (Exception)
                {
                    sMessage = "Could not update Purpose Amounts";
                    throw;
                }
            }
            return sMessage;
        }

        public List<DmoResults> FetchDmoResults(string cicoid, string fiscalYr, string atCd)
        {
            var varDmoResults = _hmEntities.GetDMOContracts(cicoid, fiscalYr, atCd).ToList();

            if (varDmoResults.Any())
            {
                var lstDmoResults = varDmoResults.AsEnumerable().Select(item =>
                    new DmoResults()
                    {
                        DmoEntityType = item.Project_Type,
                        DmoContractId = item.Contract_ID,
                        DmoEntity = item.Entity
                    }).ToList();
                return lstDmoResults;
            }
            var dmoResultses = new List<DmoResults>();
            for (var i = 0; i < 3; i++)
            {
                var dmo = new DmoResults {DmoEntityType = "DMO", DmoContractId = 0, DmoEntity = ""};
                dmoResultses.Add(dmo);
            }
            return dmoResultses;
        }

        public int UpdateDmoResults(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel)
        {
            var varDmo = formBViewmodel.DmoResultses;
            var retVal = 0;

            foreach (var dmo in varDmo)
            {
                switch (dmo.DmoEntity)
                {
                    case null:
                        dmo.DmoUpdType = 2;
                        break;
                }
                if (dmo.DmoContractId == 0 || dmo.DmoEntity == null)
                {
                }
                else
                {
                    if (dmo.DmoEntity != null && (dmo.DmoContractId > 0 && dmo.DmoEntity.Trim() != ""))
                    {
                        dmo.DmoUpdType = 1;
                    }
                    else if (dmo.DmoEntity != null && (dmo.DmoContractId > 0 && dmo.DmoEntity.Trim() == ""))
                    {
                        dmo.DmoUpdType = 2;
                    }
                    else
                    {
                        dmo.DmoUpdType = 0;
                    }
                }
                try
                {
                    _hmEntities.UpdateDMOContracts(cicoid, sYear, atCode, dmo.DmoContractId, dmo.DmoEntity,
                        Convert.ToByte(dmo.DmoUpdType));
                    retVal = 1;
                }
                catch (Exception)
                {
                    retVal = 0;
                    throw;
                }
            }
            return retVal;
        }

        public List<DmoResults> AddDmoContract(string cicoid, string sYear, string atCode)
        {
            var varDmoResults = _hmEntities.GetDMOContracts(cicoid, sYear, atCode).ToList();
            var lstDmoResults = new List<DmoResults>();

            if (varDmoResults.Any())
            {
                lstDmoResults = varDmoResults.AsEnumerable().Select(item =>
                    new DmoResults()
                    {
                        DmoEntityType = item.Project_Type,
                        DmoContractId = item.Contract_ID,
                        DmoEntity = item.Entity
                    }).ToList();

                for (var i = 0; i < 3; i++)
                {
                    var dmo = new DmoResults { DmoEntityType = "DMO", DmoContractId = 0, DmoEntity = "" };
                    lstDmoResults.Add(dmo);
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    var dmo = new DmoResults { DmoEntityType = "DMO", DmoContractId = 0, DmoEntity = "" };
                    lstDmoResults.Add(dmo);
                }
            }
            return lstDmoResults;
        }


        public List<ParksResults> FetchParksResults(string cicoid, string fiscalYr, string atCd)
        {
            var varParksResults = _hmEntities.GetParkContracts(cicoid, fiscalYr, atCd).ToList();

            if (varParksResults.Any())
            {
                var lstParksResults = varParksResults.AsEnumerable().Select(item =>
                    new ParksResults()
                    {
                       ParksEntityType = item.Project_Type,
                       ParksContractId = item.Contract_ID,
                       ParksEntity = item.Entity
                    }).ToList();
                return lstParksResults;
            }
            var parksResultses = new List<ParksResults>();
            for (var i = 0; i < 3; i++)
            {
                var parks = new ParksResults { ParksEntityType = "Park", ParksContractId = 0, ParksEntity = "" };
                parksResultses.Add(parks);
            }
            return parksResultses;
        }

        public int UpdateParksResults(string cicoid, string sYear, string atCode, FormBViewmodel formBViewmodel)
        {
            var varParks = formBViewmodel.ParkResultses;
            var retVal = 0;

            foreach (var park in varParks)
            {
                switch (park.ParksEntity)
                {
                    case null:
                        park.ParksUpdType = 2;
                        break;
                }
                if (park.ParksContractId == 0 || park.ParksEntity == null)
                {
                }
                else
                {
                    if (park.ParksEntity != null && (park.ParksContractId > 0 && park.ParksEntity.Trim() != ""))
                    {
                        park.ParksUpdType = 1;
                    }
                    else if (park.ParksEntity != null && (park.ParksContractId > 0 && park.ParksEntity.Trim() == ""))
                    {
                        park.ParksUpdType = 2;
                    }
                    else
                    {
                        park.ParksUpdType = 0;
                    }
                }
                try
                {
                    _hmEntities.UpdateParksContracts(cicoid, sYear, atCode, park.ParksContractId, park.ParksEntity,
                        Convert.ToByte(park.ParksUpdType));
                    retVal = 1;
                }
                catch (Exception)
                {
                    retVal = 0;
                    throw;
                }
            }
            return retVal;
        }

        public List<ParksResults> AddParksContract(string cicoid, string sYear, string atCode)
        {
            var varParksResults = _hmEntities.GetParkContracts(cicoid, sYear, atCode).ToList();
            var lstParksResults = new List<ParksResults>();

            if (varParksResults.Any())
            {
                lstParksResults = varParksResults.AsEnumerable().Select(item =>
                    new ParksResults()
                    {
                        ParksEntityType = item.Project_Type,
                        ParksContractId = item.Contract_ID,
                        ParksEntity = item.Entity
                    }).ToList();
                for (var i = 0; i < 3; i++)
                {
                    var parks = new ParksResults { ParksEntityType = "Park", ParksContractId = 0, ParksEntity = "" };
                    lstParksResults.Add(parks);
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    var parks = new ParksResults { ParksEntityType = "Park", ParksContractId = 0, ParksEntity = "" };
                    lstParksResults.Add(parks);
                }
            }
            return lstParksResults;
        }

        public int UpdateP2Certification(string cicoid, string sYear, string atCode, bool p2Complete)
        {
            var retval = 0;
            try
            {
                _hmEntities.UpdateFormBStatus(atCode, cicoid, sYear, p2Complete);
                retval = 1;
                return retval;
            }
            catch (Exception)
            {
                retval = 0;
                return retval;
                throw;
            }
        }
    }
}
