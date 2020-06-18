using System;
using System.Web.Mvc;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.ViewModels;
using HotelMotelTaxReporting.Common;

namespace HotelMotelTaxReporting.Controllers
{
    public class PrintController : Controller
    {
#region private properties
        
        private readonly IHmFormOne _formOneRepository;
        private readonly IFormB _formBRepository;
        private readonly IFormC _formCRepository;
        private readonly ICert _certRepository;
        private readonly HmUtils _utils;

        #endregion private properties

#region Constructor
        public PrintController(IHmFormOne formOneResults, IFormB formBResults, IFormC formCResults, ICert certRepository)
        {
            _formOneRepository = formOneResults;
            _formBRepository = formBResults;
            _formCRepository = formCResults;
            _certRepository = certRepository;

            _utils = new HmUtils();
        }
#endregion Constructor
        [Authorize]
        public ActionResult Form1()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult) View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession() && ViewBag.sYear == null)
            {
                return _utils.GetMmSession() ? (ActionResult) View() : RedirectToAction("Dashboard", "Hm");
            }

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            ViewBag.sYear = _utils.GetTaxyear();
            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            var varFormOneResults = _formOneRepository.FetchFormOneData(Session["LoginId"].ToString(),
                _utils.GetTaxyear());
            return View("Form1", varFormOneResults);
        }

        [Authorize]
        public ActionResult Form2()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varPurposeAmts = _formBRepository.FetchPurposeAmount(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
            var varDmoResults = _formBRepository.FetchDmoResults(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());
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
/*
        [Authorize]
        public ActionResult Form3()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
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
*/
        [Authorize]
        public ActionResult Form3()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varForm3 = _formCRepository.FetchFileDetails(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            ViewBag.DocExists = varForm3 != null;

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(varForm3);
        }

        [Authorize]
        public ActionResult Certification()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!Convert.ToBoolean(Session["CertificationComplete"]))
            {
                return RedirectToAction("Mainmenu", "Hm");
            }

            ViewBag.sYear = _utils.GetTaxyear();

            var varActionName = RouteData.Values["action"].ToString();
            ViewBag.ActName = varActionName;

            var varCert = _certRepository.FetchCertification(Session["LoginId"].ToString(), _utils.GetTaxyear(), Session["AtCode"].ToString());

            TempData["YearValid"] = true;
            TempData["TaxYear"] = Session["TaxYear"].ToString();

            return View(varCert);
        }















/*

        [Authorize]
        public ActionResult Form3Orig()
        {
            if (!_utils.IsAuthorized())
            {
                return _utils.IsAuthorized() ? (ActionResult)View() : RedirectToAction("Index", "Hm");
            }
            if (!_utils.GetMmSession())
            {
                return _utils.GetMmSession() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
            }
            if (!_utils.IsAuthorisedForView())
            {
                return _utils.IsAuthorisedForView() ? (ActionResult)View() : RedirectToAction("Dashboard", "Hm");
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
 */
    }
}
