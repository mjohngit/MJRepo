using System;
using System.Collections.Generic;
using System.Linq;
using HotelMotel.Business.ViewModels;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.Repository
{
    public class Form3Repository : IForm3
    {
        private readonly HotelMotelTaxEntities _hmEntities = new HotelMotelTaxEntities();

        public List<Pcis> FetchPcises(string cicoid, string fiscalYr, string atCd, bool blnPrint)
        {
            var varPcisResults = _hmEntities.FetchPcisList(cicoid, fiscalYr, atCd).ToList();

            if (blnPrint)
            {
                var lstPcisResults = varPcisResults.AsEnumerable().Select(item =>
                        new Pcis()
                        {
                            PcisId = item.PCIS_ID,
                            Entity = item.Entity,
                            EntityProject = item.Entity_Project_Name,
                            PromotionItems = item.Promotion_Items,
                            ExpenditureAmount = item.Expenditure_Amount
                        }).ToList();
                return lstPcisResults;
            }
            if (varPcisResults.Any())
            {
                var lstPcisResults = varPcisResults.AsEnumerable().Select(item =>
                    new Pcis()
                    {
                        PcisId = item.PCIS_ID,
                        Entity = item.Entity,
                        EntityProject = item.Entity_Project_Name,
                        PromotionItems = item.Promotion_Items,
                        ExpenditureAmount = item.Expenditure_Amount
                    }).ToList();
                return lstPcisResults;
            }


            var lstPcis = new List<Pcis>();
            for (var i = 0; i < 25; i++)
            {
                var pcis = new Pcis { PcisId = 0, Entity = "", EntityProject = "", PromotionItems = "", ExpenditureAmount = 0};
                lstPcis.Add(pcis);
            }
            return lstPcis;
        }

        public int UpdatePcises(string cicoid, string sYear, string atCode, FormCViewmodel formCViewmodel)
        {
            var varPcis = formCViewmodel.Pcises;
            var retVal = 0;

            foreach (var pc in varPcis)
            {
                switch (pc.Entity)
                {
                    case null:
                        pc.PcisUpdType = 2;
                        break;
                }
                if (pc.PcisId == 0 || pc.Entity == null)
                {
                }
                else
                {
                    if (pc.Entity != null && (pc.PcisId > 0 && pc.Entity.Trim() != ""))
                    {
                        pc.PcisUpdType = 1;
                    }
                    else if (pc.Entity != null && (pc.PcisId > 0 && pc.Entity.Trim() == ""))
                    {
                        pc.PcisUpdType = 2;
                    }
                    else
                    {
                        pc.PcisUpdType = 0;
                    }
                }
                try
                {
                    _hmEntities.UpdatePcis(cicoid, sYear, atCode, pc.PcisId, pc.Entity, pc.EntityProject, pc.PromotionItems, pc.ExpenditureAmount, Convert.ToByte(pc.PcisUpdType));
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

        public List<Pcis> AddPcises(string cicoid, string sYear, string atCode)
        {
            var varPcisResults = _hmEntities.FetchPcisList(cicoid, sYear, atCode).ToList();
            var lstPcisResults = new List<Pcis>();


            if (varPcisResults.Any())
            {
                lstPcisResults = varPcisResults.AsEnumerable().Select(item =>
                    new Pcis()
                    {
                        PcisId = item.PCIS_ID,
                        Entity = item.Entity,
                        EntityProject = item.Entity_Project_Name,
                        PromotionItems = item.Promotion_Items,
                        ExpenditureAmount = item.Expenditure_Amount
                    }).ToList();

                for (var i = 0; i < 5; i++)
                {
                    var pcis = new Pcis { PcisId = 0, Entity = "", EntityProject = "", PromotionItems = "", ExpenditureAmount = 0 };
                    lstPcisResults.Add(pcis);
                }
            }
            else
            {
                var pcis = new Pcis { PcisId = 0, Entity = "", EntityProject = "", PromotionItems = "", ExpenditureAmount = 0 };
                lstPcisResults.Add(pcis);
            }
            return lstPcisResults;
        }

        public List<Project> FetchProjects(string cicoid, string fiscalYr, string atCd, bool blnPrint)
        {
            var varProjectResults = _hmEntities.FetchProjectList(cicoid, fiscalYr, atCd).ToList();

            if (blnPrint)
            {
                var lstProjects = varProjectResults.AsEnumerable().Select(item =>
                    new Project()
                    {
                        ProjectId = item.Project_ID,
                        ProjectDescription = item.Description,
                        ProjectStatus = item.Project_Status
                    }).ToList();

                return lstProjects;
            }
            if (varProjectResults.Any())
            {
                var lstProjects = varProjectResults.AsEnumerable().Select(item =>
                    new Project()
                    {
                        ProjectId = item.Project_ID,
                        ProjectDescription = item.Description,
                        ProjectStatus = item.Project_Status
                    }).ToList();

                return lstProjects;
            }


            var lstProj= new List<Project>();
            for (var i = 0; i < 5; i++)
            {
                var proj = new Project { ProjectId = 0, ProjectDescription = "", ProjectStatus = ""};
                lstProj.Add(proj);
            }
            return lstProj;
        }

        public int UpdateProjects(string cicoid, string sYear, string atCode, FormCViewmodel formCViewmodel)
        {
            var varProjects = formCViewmodel.Projects;
            var retVal = 0;

            foreach (var proj in varProjects)
            {
                switch (proj.ProjectDescription)
                {
                    case null:
                        proj.ProjectUpdType = 2;
                        break;
                }
                if (proj.ProjectId == 0 || proj.ProjectDescription == null)
                {
                }
                else
                {
                    if (proj.ProjectDescription != null && (proj.ProjectId > 0 && proj.ProjectDescription.Trim() != ""))
                    {
                        proj.ProjectUpdType = 1;
                    }
                    else if (proj.ProjectDescription != null && (proj.ProjectId > 0 && proj.ProjectDescription.Trim() == ""))
                    {
                        proj.ProjectUpdType = 2;
                    }
                    else
                    {
                        proj.ProjectUpdType = 0;
                    }
                }
                try
                {
                    _hmEntities.UpdateProjects(cicoid, sYear, atCode, proj.ProjectId, proj.ProjectDescription, proj.ProjectStatus, Convert.ToByte(proj.ProjectUpdType));
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

        public List<Project> AddProjects(string cicoid, string sYear, string atCode)
        {
            var varProjectResults = _hmEntities.FetchProjectList(cicoid, sYear, atCode).ToList();
            var lstProjectResults = new List<Project>();

            if (varProjectResults.Any())
            {
                lstProjectResults = varProjectResults.AsEnumerable().Select(item =>
                    new Project()
                    {
                        ProjectId = item.Project_ID,
                        ProjectDescription = item.Description,
                        ProjectStatus = item.Project_Status
                    }).ToList();

                for (var i = 0; i < 5; i++)
                {
                    var proj = new Project { ProjectId = 0, ProjectDescription = "", ProjectStatus = ""};
                    lstProjectResults.Add(proj);
                }
            }
            else
            {
                var projs = new Project { ProjectId = 0, ProjectDescription = "", ProjectStatus = ""};
                lstProjectResults.Add(projs);
            }
            return lstProjectResults;
        }

        public List<ProjectStatus> FetchProjectStatuses()
        {
            var varStatus = _hmEntities.FetchProjectStatusList().ToList();

            var lstProjStatuses = varStatus.AsEnumerable().Select(item =>
                new ProjectStatus()
                {
                    ProjStatus = item.ProjectStatus
                }).ToList();

            return lstProjStatuses;
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
                throw;
            }
        }
    
    }
}
