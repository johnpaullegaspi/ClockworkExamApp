using Clockwork.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
        string _url = string.Empty;
        DataContainer _returnedData = null;

        public HomeController()
        {
            _url = Common.GetConfigurationValue("GetServiceDomainURL");
        }

        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;
            
            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            try
            {
                _returnedData = Common.GetDataResult<DataContainer>(_url, "GetCurrentTimeQueries", HttpType.GET, null);
            }
            catch (Exception ex)
            {

            }

            return View(_returnedData);
        }

        [HttpGet]
        public ActionResult AddTimeByTimeZone(string timeZoneId)
        {

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("timeZoneId", timeZoneId);

            try
            {
                _returnedData = Common.GetDataResult<DataContainer>(_url, "AddTimeByTimeZone", HttpType.GET, data);
            }
            catch (Exception ex)
            {

            }
            return Json(_returnedData.Data, JsonRequestBehavior.AllowGet);
        }

       
    }
}
