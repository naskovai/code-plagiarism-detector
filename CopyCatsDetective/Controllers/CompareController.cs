using CopyCatsDetective.Controllers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CopyCatsDetective.Controllers
{
    public class CompareController : Controller
    {
        //
        // GET: /Compare/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CompareCodes()
        {
            string firstSourceCode = Request.Form["FirstSourceCode"];
            string secondSourceCode = Request.Form["SecondSourceCode"];
            CodePlagiarismDetector detector = new CodePlagiarismDetector();
            var result =  await detector.Compare(Languages.CSharp, firstSourceCode, secondSourceCode);
            IEnumerable<CodePlagiarismDetectionResult> results = new[] { result };
            return View(results);
        }
	}
}