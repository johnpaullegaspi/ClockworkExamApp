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
                _returnedData = new DataContainer();
                _returnedData.Message = Common.CheckMessage(ex.Message);
            }

            return View(_returnedData);
        }

        [HttpGet]
        public ActionResult AddTimeByTimeZone(string timeZoneId)
        {

            var data = new Dictionary<string, string>();

            data.Add("timeZoneId", timeZoneId);

            try
            {
                _returnedData = Common.GetDataResult<DataContainer>(_url, "AddTimeByTimeZone", HttpType.GET, data);
            }
            catch (Exception ex)
            {
                _returnedData = new DataContainer();
                _returnedData.Message = Common.CheckMessage(ex.Message);
            }
            return Json(_returnedData, JsonRequestBehavior.AllowGet);
        }

        public void FirstCommit()
        {
            string name = "Hello";
        }

        public void PullRequest()
        {
            string name = "Request for pull request.";
        }

        public void NewBranch()
        {
            string name = "new branch.";
        }

        public void Sync()
        {
            string done = "Done.";
        }

    }

}
