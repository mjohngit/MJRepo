using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HotelMotel.Business.IFacade;
using HotelMotelTaxReporting.Common;

namespace HotelMotelTaxReporting.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHmLogin _loginRepository;
        private readonly HmUtils _utils;

        public HomeController(IHmLogin loginResults)
        {
            _loginRepository = loginResults;
            _utils = new HmUtils();
        }

        public ActionResult Index(IHmLogin loginResults)
        {
            ViewBag.Message = "Hotel-Motel Tax Reporting System Login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string loginId, string password)
        {
            var v = _loginRepository.LogResult(loginId, password);

            //if (v.Cico != 2 && v.AnnexLoginResult == "OK")
            //{
            //    ViewBag.LoginError = "Only City governments can report annexation details";
            //    //_utils.AbandonSession(loginId);
            //    FormsAuthentication.SignOut();
            //    return View();
            //}
            //else if (v.AnnexLoginResult == "OK")
            if (v.AnnexLoginResult == "OK")
            {
                _utils.CreateLiveSession(v.CicoId.ToString(CultureInfo.InvariantCulture),
                 v.Cico.ToString(CultureInfo.InvariantCulture), v.EntityName, v.AnnexLoginFips);
                return RedirectToAction("ArStart");
            }
            else if (v.AnnexLoginResult == "XL")
            {
                _utils.AbandonSession(loginId);
                ViewBag.LoginError = "Login not found";
            }
            else if (v.AnnexLoginResult == "XP")
            {
                _utils.AbandonSession(loginId);
                ViewBag.LoginError = "Password does not match Login";
            }
            return View();
        }

        [Authorize]
        public ActionResult ArStart()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            ViewBag.Message = "Successfully Loggod Out.";
            return RedirectToAction("Index");
        }
    }
}