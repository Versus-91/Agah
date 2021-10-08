using AFIAT.TST.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AFIAT.TST.Web.Public.Controllers
{
    [Route("{controller}")]
    public class PagesController : TSTControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("{id}")]
        public ActionResult Show()
        {
            return View();
        }
    }
}
