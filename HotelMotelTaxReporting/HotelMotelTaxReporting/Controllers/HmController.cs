using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;
using HotelMotel.Business.ViewModels;
using HotelMotelTaxReporting.Common;
using MVCLibraries;

namespace HotelMotelTaxReporting.Controllers
{
    public class HmController : Controller
    {
#region private properties
        private readonly IHmLogin _loginRepository;
        private readonly IHmDashboard _dashboardRepository;
        private readonly IHmMenu _mainMenuRepository;
        private readonly IHmFormOne _formOneRepository;
        private readonly IFormB _formBRepository;
        private readonly IFormC _formCRepository;
        private readonly ICert _certRepository;

        private readonly HmUtils _utils;
        private string _strLoginId;

        #endregion private properties

#region Constructor
        public HmController(IHmLogin loginResults, IHmDashboard dashbordResults, IHmMenu mainMenuResults, IHmFormOne formOneResults, IFormB formBResults, IFormC formCResults, ICert certResults)
        {
            _loginRepository = loginResults;
            _dashboardRepository = dashbordResults;
            _mainMenuRepository = mainMenuResults;
            _formOneRepository = formOneResults;
            _formBRepository = formBResults;
            _formCRepository = formCResults;
            _certRepository = certResults;

            _utils = new HmUtils();
            _strLoginId = "0";
        }
#endregion Constructor

#region Action Index
        public ActionResult Index()
        {
            ViewBag.Message = "Hotel-Motel Tax Reporting System Login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string loginId, string password)
        {
            _strLoginId = loginId;
            var v = _loginRepository.LogResult(loginId, password);

            if (v.AnnexLoginResult == "OK")
            {
                _utils.CreateLiveSession(v.CicoId.ToString(CultureInfo.InvariantCulture),
                 v.Cico.ToString(CultureInfo.InvariantCulture), v.EntityName, v.AnnexLoginFips);
                return RedirectToAction("Dashboard");
            }
            if (v.AnnexLoginResult == "XL")
            {
                _utils.AbandonSession(loginId);
                ViewBag.LoginError = "Login not found";
            }
            else if (v.AnnexLoginResult == "XP")
            {
                _utils.AbandonSession(loginId);
                ViewBag.LoginError = "Password does not match Login";
            }
            Session["LogoutMessage"] = "";

            return View();
        }
#endregion Action Index

#region Action Dashboard
        [Authorize]
        public ActionResult Dashboard()
        {
            if (Session["LoginId"] != null)
            {
                var varHmDashboard = _dashboardRepository.DashboardResults(Session["LoginId"].ToString());
                return View(varHmDashboard);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Dashboard(string surveyYear)
        {
            return View("Mainmenu");
        }
#endregion Action Dashboard

#region Action Mainmenu
        [Authorize]
        public ActionResult Mainmenu()
        {
            ViewBag.sYear = Request["hidSurveyYear"];
            _utils.SetMMSession(Request.Form["hidSurveyYear"]);
            

            if (!_utils.IsAuthorized())
                return RedirectToAction("Index");

            if (Convert.ToBoolean(TempData["YearValid"]) || ViewBag.sYear != null)
            {
                ViewBag.sYear = (TempData["TaxYear"] ?? ViewBag.sYear) ?? Request["syear"].ToString(CultureInfo.InvariantCulture);

                var varMainmenu = _mainMenuRepository.FetchMainmenu(Session["LoginId"].ToString(), ViewBag.sYear);

                if (varMainmenu != null)
                {
                    PropertyInfo pi = varMainmenu.GetType().GetProperty("OrdinanceCorrect");
                    Session["OrdinanceCorrect"] = (bool) (pi.GetValue(varMainmenu, null));

                    PropertyInfo piOne = varMainmenu.GetType().GetProperty("CertificationComplete");
                    Session["CertificationComplete"] = (bool) (piOne.GetValue(varMainmenu, null));

                    PropertyInfo piTwo = varMainmenu.GetType().GetProperty("AtCode");
                    Session["AtCode"] = piTwo.GetValue(varMainmenu, null);

                    PropertyInfo piThree = varMainmenu.GetType().GetProperty("P2Complete");
                    Session["P2Complete"] = piThree.GetValue(varMainmenu, null);

                    PropertyInfo piFour = varMainmenu.GetType().GetProperty("P3Complete");
                    Session["P3Complete"] = piFour.GetValue(varMainmenu, null);
                }

                Session["TaxYear"] = ViewBag.sYear ?? TempData["TaxYear"];
                
                TempData["YearValid"] = null;
                TempData["TaxYear"] = null;

                return View("Mainmenu", varMainmenu);
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult) View() : RedirectToAction("Dashboard");
            }
            return View();
        }
#endregion Action Mainmenu

#region Action Form1
        [Authorize]
        public ActionResult Form1()
        {  
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult) View() : RedirectToAction("Index");
            }
            if (!_utils.GetMmSession() && ViewBag.sYear == null)
            {
                return _utils.GetMmSession() ? (ActionResult) View() : RedirectToAction("Dashboard");
            }

            var varActionName =  RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.sYear = _utils.GetTaxyear();
            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            var varFormOneResults = _formOneRepository.FetchFormOneData(Session["LoginId"].ToString(), _utils.GetTaxyear());
            return View(varFormOneResults);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Form1Save()
        {
            var blnOrd = Convert.ToBoolean(Request["OrdCorrect"]);
            var blnShortTermRentals = Convert.ToBoolean(Request["ShortTermRentals"]);
            var strAtCode = Request["AtCode"];

            _formOneRepository.UpdateP1Certification(Session["LoginId"].ToString(), sYear: Session["TaxYear"].ToString(), atCode: strAtCode, p1Complete: true, ordCorrect: blnOrd, shortTermRentals: blnShortTermRentals);

            if (blnOrd)
                _formOneRepository.FillPurposeAmounts(Session["LoginId"].ToString(), Session["TaxYear"].ToString(),
                    strAtCode);
            
            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return RedirectToAction("Mainmenu");
        }
#endregion Action Mainmenu

#region Form2 GET
        [Authorize]
        public ActionResult Form2()
        {
            if (!_utils.IsAuthorized())
                return _utils.IsAuthorized() ? (ActionResult) View() : RedirectToAction("Index");
            if (!_utils.GetMmSession())
                return _utils.GetMmSession() ? (ActionResult) View() : RedirectToAction("Dashboard");
            if (!_utils.IsAuthorisedForView())
                return _utils.IsAuthorisedForView() ? (ActionResult) View() : RedirectToAction("Dashboard");

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varPurposeAmts = _formBRepository.FetchPurposeAmount(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            var varDmoResults = _formBRepository.FetchDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(),Session["AtCode"].ToString());
            var varParksResults = _formBRepository.FetchParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var formBvm = new FormBViewmodel
            {
                PurposeAmountResults = varPurposeAmts,
                DmoResultses = varDmoResults,
                ParkResultses = varParksResults
            };

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formBvm);
        }
#endregion Form2 GET

#region Regular POST form -- This was used before implementation of AJAX but will full refresh
        [Authorize]
        [ActionName("Form2")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "btnSavePurposeAmts", Value = "Save Purpose Amounts")]
        public ActionResult PurposeAmounts_Post(FormBViewmodel fbvm)
        {
            var sMessage =  _formBRepository.UpdatePurposeAmounts(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);
            //var varDmoResults = _formBRepository.FetchDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            //return Json(fbvm, JsonRequestBehavior.AllowGet);
            return View(fbvm);
        }

        [Authorize]
        [ActionName("Form2")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "btnSaveDmo", Value = "Save DMO Section")]
        public JsonResult DMO_Post(FormBViewmodel fbvm)
        {
            var sMessage = _formBRepository.UpdateDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            ModelState.Clear();
            var varDmoResults = _formBRepository.FetchDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            fbvm.DmoResultses = varDmoResults;

            return Json(fbvm);
            //return Json(fbvm, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[ActionName("Form2")]
        //[AcceptVerbs(HttpVerbs.Post)]
        //[AcceptParameter(Name = "btnAddDmoContract", Value = "Add new row ")]
        //public ActionResult DMO_Add(FormBViewmodel fbvm)
        //{
        //    var lstDmo = _formBRepository.AddDmoContract(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);

        //    var varActionName = RouteData.Values["action"].ToString();
        //    ViewBag.ActName = varActionName;
            
        //    TempData["YearValid"] = true;
        //    TempData["TaxYear"] = Session["TaxYear"].ToString();

        //    ModelState.Clear();
        //    fbvm.DmoResultses = lstDmo;

        //    //return View(fbvm);
        //    return Json(fbvm, JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        [ActionName("Form2")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "btnSavePark", Value = "Save Parks Section")]
        public ActionResult Parks_Post(FormBViewmodel fbvm)
        {
            var sMessage = _formBRepository.UpdateParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            ModelState.Clear();
            var varParksResults = _formBRepository.FetchParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            fbvm.ParkResultses = varParksResults;

            return View(fbvm);
            //return Json(fbvm, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[ActionName("Form2")]
        //[AcceptVerbs(HttpVerbs.Post)]
        //[AcceptParameter(Name = "btnAddParksContract", Value = "Add new row")]
        //public ActionResult Parks_Add(FormBViewmodel fbvm)
        //{
        //    var lstParks = _formBRepository.AddParksContract(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

        //    var varActionName = RouteData.Values["action"].ToString();
        //    ViewBag.ActName = varActionName;

        //    TempData["YearValid"] = true;
        //    TempData["TaxYear"] = Session["TaxYear"].ToString();

        //    ModelState.Clear();
        //    fbvm.ParkResultses = lstParks;

        //    return View(fbvm);
        //    //return Json(fbvm, JsonRequestBehavior.AllowGet);
        //}
#endregion Regular POST form -- This was used before implementation of AJAX but will full refresh

#region Partials for Form2
        public PartialViewResult AddPartialNewContractDmo()
        {
            var lstDmo = _formBRepository.AddDmoContract(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var fbvm = new FormBViewmodel { DmoResultses = lstDmo };

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return PartialView("Partials/_ContractDmo", fbvm);
        }

        public PartialViewResult AddPartialNewContractParks()
        {
            var lstParks = _formBRepository.AddParksContract(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            
            ModelState.Clear();
            var fbvm = new FormBViewmodel {ParkResultses = lstParks};

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            ModelState.Clear();
            fbvm.ParkResultses = lstParks;

            return PartialView("Partials/_ContractParks", fbvm);
        }
#endregion end Partials for Form2
        
#region Save the partial as Ajax Asynchronous

        [Authorize]
        public PartialViewResult SavePartialPurposeAmounts(FormBViewmodel formBvm)
        {
            var fbvm = new FormBViewmodel { PurposeAmountResults = formBvm.PurposeAmountResults};
            var sMessage =  _formBRepository.UpdatePurposeAmounts(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), formBvm);

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return PartialView("Partials/_PurposeAmounts", fbvm);
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult SavePartialDmoResults(FormBViewmodel formBvm)
        {
            var fbvm = new FormBViewmodel { DmoResultses = formBvm.DmoResultses};
            var sMessage = _formBRepository.UpdateDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);

            var varDmoResults = _formBRepository.FetchDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            fbvm.DmoResultses = varDmoResults;

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();
            
            return PartialView("Partials/_ContractDmo", fbvm);
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult SavePartialParksResults(FormBViewmodel formBvm)
        {
            var fbvm = new FormBViewmodel { ParkResultses = formBvm.ParkResultses };

            var sMessage = _formBRepository.UpdateParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvm);
            
            var varParkResults = _formBRepository.FetchParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            fbvm.ParkResultses = varParkResults;

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.SuccessMessage = sMessage;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return PartialView("Partials/_ContractParks", fbvm);
        }
#endregion Save the partial as Ajax Asynchronous

#region Form2 Save
        [Authorize]
        [ActionName("Form2")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "btnSubmit", Value = "Save Section II")]
        public ActionResult Form2_Post(FormBViewmodel fbvmViewmodel)
        {
            if (Session != null)
                _formBRepository.UpdatePurposeAmounts(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvmViewmodel);
            if (Session != null)
                _formBRepository.UpdateDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvmViewmodel);
            if (Session != null)
                _formBRepository.UpdateParksResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fbvmViewmodel);
            if (Session != null)
                _formBRepository.UpdateP2Certification( Session["TaxYear"].ToString(), Session["AtCode"].ToString(), Session["LoginId"].ToString(), true);

            TempData["YearValid"] = true;
            TempData["TaxYear"] = _utils.GetTaxyear();
          
            //return Json("Mainmenu");
            return RedirectToAction("Mainmenu");
        }
#endregion Form 2 Save

#region Form3 GET - Changed specs
        [Authorize]
        public ActionResult Form3()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard");
            }

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());

            var varForm3 = _formCRepository.FetchFileDetails(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (varForm3 != null)
            {
                foreach (var pics in varForm3) {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis; 
            //ViewBag.DocExists = varForm3 != null && varForm3.PcisDocId > 0;
            ViewBag.DocExists = varForm3 != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(varForm3);
        }
#endregion Form3 GET - Changed specs

#region Form3 Upload and Save - Changed specs

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "updSubmit", Value = "Upload Document")]
        public ActionResult Form3FileUpload(IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            var di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("../") + "/Uploads/");

            foreach (string upload in Request.Files)
            {
                if (!Request.Files[upload].HasFile()) continue;

                var filename = Path.GetFileName(Request.Files[upload].FileName);
                {
                    var extension = Path.GetExtension(Request.Files[upload].FileName);

                    if (extension == ".xls" || extension == ".xlsx" || extension == ".doc" || extension == ".docx" ||
                        extension == ".txt" || extension == ".csv" || extension == ".pdf" || extension == ".eml")
                    {
                        if (System.IO.File.Exists(di.ToString() + filename))
                        {
                            ViewBag.Message = "File already exists!";
                        }
                        else
                        {
                            ViewBag.Message = "";
                            try
                            {
                                if (HttpContext.Session != null)
                                {
                                    var strMappath = di.ToString() + "/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear();
                                    var filePath = "/Uploads/" + "/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear();

                                    var dis = new DirectoryInfo(di.ToString());

                                    if (!Directory.Exists(strMappath))
                                    {
                                        var ds = Directory.CreateDirectory(strMappath);
                                        dis = new DirectoryInfo(strMappath);
                                    }
                                    else
                                    {
                                        dis = new DirectoryInfo(strMappath);
                                    }

                                    var httpPostedFileBase = Request.Files[upload];
                                    if (httpPostedFileBase != null)
                                        if (filename != null)
                                            httpPostedFileBase.SaveAs(Path.Combine(dis.ToString(), filename));

                                    _formCRepository.AddUploadFileDbEntry(Session["LoginId"].ToString(), _utils.GetTaxyear(),
                                    Session["AtCode"].ToString(), filename, filePath);
                                }
                                ViewBag.Message = "File uploaded successfully!!!";
                                ViewBag.clrMsg = "G";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = "Error! Couldn't upload document" + ex.Message.ToString();
                                ViewBag.clrMsg = "R";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Cannot upload files with " + extension + " extension.";
                        ViewBag.clrMsg = "BR";
                    }
                }
            }

            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var form3 = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(),_utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (form3 != null)
            {
                foreach (var pics in form3)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = form3 != null && formc.PcisDocId > 0;
            ViewBag.DocExists = form3 != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(form3);
        }

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "frmDelete0", Value = "Delete")]
        public ActionResult DeleteDocument(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            if (HttpContext.Session != null)
            {
                var fullPath = Request.MapPath("~/Uploads/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear() + "/" + Request["item.PcisDocName"]);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (Session != null)
                // _formCRepository.DeleteFileNDbrow(form3);  -- Changed to Request["ControlName"] since not getting Form3
                _formCRepository.DeleteFileNDbrow(int.Parse(Request["item.PcisDocId"].ToString()));
            if (HttpContext.Session == null) return View();

            if (Session == null) return View();
            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var formc = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var FormC = new Form3();
            var docCount = 0;

            if (form3 != null)
            {
                foreach (var pics in formc)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = formc != null //&& formc.PcisDocId > 0;
            ViewBag.DocExists = formc != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formc);
        }

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "btnSubmit", Value = "Save Section III")]
        public ActionResult Form3(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();
            if (Session != null)
                _formCRepository.UpdateP3Certification(Session["TaxYear"].ToString(), Session["AtCode"].ToString(), Session["LoginId"].ToString(), true);

            TempData["YearValid"] = true;
            TempData["TaxYear"] = _utils.GetTaxyear();

            //return Json("Mainmenu");
            return RedirectToAction("Mainmenu");
        }
#endregion Form3 Save - Changed specs

#region Action Certification
        [Authorize]
        public ActionResult Certification()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult) View() : RedirectToAction("Dashboard");
            }

            if (!_utils.IsAuthorizedForCertification())
            {
                return _utils.IsAuthorizedForCertification() ? (ActionResult)View() : RedirectToAction("Mainmenu");
            }

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varCert = _certRepository.FetchCertification(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(varCert);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Certification(Certification certification)
        { 
            certification.CertComplete = true;
            if (Session == null)
            {
                return RedirectToAction("HmError", "Hm");
            }
            else
                try
                {
                    _certRepository.UpdateCertification(Session["LoginId"].ToString(), _utils.GetTaxyear(),
                        Session["AtCode"].ToString(), certification);
                    _certRepository.NotifyPreparerOnSubmit(Session["LoginId"].ToString(), _utils.GetTaxyear(),
                        certification.PreparerEmail);
                }
                catch (Exception)
                {
                    throw;
                }
        
            TempData["YearValid"] = true;
            TempData["TaxYear"] = _utils.GetTaxyear();

            //return Json("Mainmenu");
            return RedirectToAction("Mainmenu");
        }

#endregion Action Certification

#region Action Logoff
        [Authorize]
        public ActionResult LogOff()
        {
            _utils.AbandonSession(_strLoginId);
            return RedirectToAction("Index");
        }
#endregion Action Logoff

#region Action Error
        [Authorize]
        public ActionResult HmError()
        {
            return View();
        }
#endregion Action Error

#region Action NotFound
        [Authorize]
        public ActionResult NotFound()
        {
            return View();
        }
        #endregion Action NotFound

        #region Partial FormMenu
        //public PartialViewResult FormMenu()
        //{
        //    return PartialView("Partials/_Menu");
        //}
        #endregion Partial FormMenu








        //Original Form 3 with PCIS and Project classes -- This was replaced with the Upload variant
        /*
        #region GET Action Form3
                [Authorize]
                public ActionResult Form3Orig()
                {
                    if (!_utils.IsAuthorized())
                    {
                        return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index");
                    }
                    if (!_utils.GetMmSession())
                    {
                        return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard");
                    }
                    if (!_utils.IsAuthorisedForView())
                    {
                        return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard");
                    }

                    ViewBag.sYear = _utils.GetTaxyear();

                    var varActionName = RouteData.Values["action"].ToString();
                    ViewBag.ActName = varActionName;

                    var varStatuses = _formCRepository.FetchProjectStatuses();
                    ViewBag.cmbStatuses = varStatuses;

                    var varPcis = _formCRepository.FetchPcises(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), Convert.ToBoolean(Session["CertificationComplete"]));
                    var varProjects = _formCRepository.FetchProjects(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), Convert.ToBoolean(Session["CertificationComplete"]));

                    var formCvm = new FormCViewmodel
                    {
                        Pcises = varPcis,
                        Projects = varProjects
                    };

                    TempData["YearValid"] = true;
                    TempData["TaxYear"] = Session["TaxYear"].ToString();

                    return View(formCvm);
                }

                public PartialViewResult _AddPartialNewPcisOrig()
                {
                    var lstPcis = _formCRepository.AddPcises(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

                    var fcvm = new FormCViewmodel { Pcises = lstPcis };

                    var varActionName = RouteData.Values["action"].ToString();
                    ViewBag.ActName = varActionName;

                    TempData["YearValid"] = true;
                    TempData["TaxYear"] = Session["TaxYear"].ToString();

                    return PartialView("Partials/_Pcis", fcvm);
                }

                public PartialViewResult _AddPartialNewProjectsOrig()
                {
                    var varStatuses = _formCRepository.FetchProjectStatuses();
                    ViewBag.cmbStatuses = varStatuses;

                    var lstProjects = _formCRepository.AddProjects(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

                    var fcvm = new FormCViewmodel { Projects = lstProjects };

                    var varActionName = RouteData.Values["action"].ToString();
                    ViewBag.ActName = varActionName;

                    TempData["YearValid"] = true;
                    TempData["TaxYear"] = Session["TaxYear"].ToString();

                    return PartialView("Partials/_Projects", fcvm);
                }

        #endregion GET Action Form3

        #region Form3 Original with Pcis & Projects Save
                [Authorize]
                [HttpPost]
                public ActionResult Form3Orig(FormCViewmodel fcvmViewmodel)
                {
                    if (Session != null)
                        _formCRepository.UpdatePcises(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fcvmViewmodel);
                    if (Session != null)
                        _formCRepository.UpdateProjects(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString(), fcvmViewmodel);
                    if (Session != null)
                        _formCRepository.UpdateP3Certification(Session["TaxYear"].ToString(), Session["AtCode"].ToString(), Session["LoginId"].ToString(), true);

                    TempData["YearValid"] = true;
                    TempData["TaxYear"] = _utils.GetTaxyear();

                    //return Json("Mainmenu");
                    return RedirectToAction("Mainmenu");
                }
        #endregion Form 3 Original with Pcis & Projects Save
        */
        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "frmDelete1", Value = "Delete")]
        public ActionResult DeleteDocument1(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            if (HttpContext.Session != null)
            {
                var fullPath = Request.MapPath("~/Uploads/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear() + "/" + Request["item.PcisDocName"]);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (Session != null)
                // _formCRepository.DeleteFileNDbrow(form3);  -- Changed to Request["ControlName"] since not getting Form3
                _formCRepository.DeleteFileNDbrow(int.Parse(Request["item.PcisDocId"].ToString()));
            if (HttpContext.Session == null) return View();

            if (Session == null) return View();
            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var formc = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (form3 != null)
            {
                foreach (var pics in formc)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = formc != null //&& formc.PcisDocId > 0;
            ViewBag.DocExists = formc != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formc);
        }

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "frmDelete2", Value = "Delete")]
        public ActionResult DeleteDocument2(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            if (HttpContext.Session != null)
            {
                var fullPath = Request.MapPath("~/Uploads/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear() + "/" + Request["item.PcisDocName"]);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (Session != null)
                // _formCRepository.DeleteFileNDbrow(form3);  -- Changed to Request["ControlName"] since not getting Form3
                _formCRepository.DeleteFileNDbrow(int.Parse(Request["item.PcisDocId"].ToString()));
            if (HttpContext.Session == null) return View();

            if (Session == null) return View();
            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var formc = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (form3 != null)
            {
                foreach (var pics in formc)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = formc != null //&& formc.PcisDocId > 0;
            ViewBag.DocExists = formc != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formc);
        }

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "frmDelete3", Value = "Delete")]
        public ActionResult DeleteDocument3(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            if (HttpContext.Session != null)
            {
                var fullPath = Request.MapPath("~/Uploads/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear() + "/" + Request["item.PcisDocName"]);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (Session != null)
                // _formCRepository.DeleteFileNDbrow(form3);  -- Changed to Request["ControlName"] since not getting Form3
                _formCRepository.DeleteFileNDbrow(int.Parse(Request["item.PcisDocId"].ToString()));
            if (HttpContext.Session == null) return View();

            if (Session == null) return View();
            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var formc = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (form3 != null)
            {
                foreach (var pics in formc)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = formc != null //&& formc.PcisDocId > 0;
            ViewBag.DocExists = formc != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formc);
        }

        [Authorize]
        [ActionName("Form3")]
        [AcceptVerbs(HttpVerbs.Post)]
        [AcceptParameter(Name = "frmDelete4", Value = "Delete")]
        public ActionResult DeleteDocument4(Form3 form3)
        {
            ViewBag.sYear = _utils.GetTaxyear();

            if (HttpContext.Session != null)
            {
                var fullPath = Request.MapPath("~/Uploads/" + HttpContext.Session["LoginId"].ToString() + "/" + _utils.GetTaxyear() + "/" + Request["item.PcisDocName"]);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            if (Session != null)
                // _formCRepository.DeleteFileNDbrow(form3);  -- Changed to Request["ControlName"] since not getting Form3
                _formCRepository.DeleteFileNDbrow(int.Parse(Request["item.PcisDocId"].ToString()));
            if (HttpContext.Session == null) return View();

            if (Session == null) return View();
            var varPcis = _formCRepository.FindPcisDocNeeded(Session["LoginId"].ToString(), _utils.GetTaxyear());
            var formc = _formCRepository.FetchFileDetails(HttpContext.Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            var docCount = 0;
            var FormC = new Form3();

            if (form3 != null)
            {
                foreach (var pics in formc)
                {
                    docCount += 1;
                    FormC.PcisDocNeeded = pics.PcisDocNeeded;
                }
            }

            ViewBag.DocCount = docCount;
            ViewBag.NeedDoc = varPcis;
            //ViewBag.DocExists = formc != null //&& formc.PcisDocId > 0;
            ViewBag.DocExists = formc != null; //changed MJ 05/28/2020 to facilitate multiple uploads;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(formc);
        }
    }
}