using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ChangeableType.Extensions;

namespace ChangeableType.Controllers
{
    // a simple model
    public class MyModel
    {
        [Display(Prompt = "My watermark")]
        public string WatermarkTest { get; set; }
        [Range(1, 10), Display(Name = "Some Integer")]
        public Changeable<int> SomeInt { get; set; }
        [StringLength(32, MinimumLength = 6), Display(Name = "This String")]
        public Changeable<string> TheString { get; set; }
    } 


    
    public class ChangeableController : Controller
    {

        public ActionResult Index()
        {
            var myModel = new MyModel
            {
                SomeInt = new Changeable<int> { Value = 5 },
                TheString = new Changeable<string> { Value = "something" }
            };
            return View(myModel);
        }

        [HttpPost]
        public ActionResult Index(MyModel myModel)
        {
            if (ModelState.IsValid)
            {
                // save model

                // return index action
                return RedirectToAction("Index");
            }

            // return invalid model
            return View(myModel);
        }
    }
}
