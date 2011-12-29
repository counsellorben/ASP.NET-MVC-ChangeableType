using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChangeableType.Extensions
{
    public class ChangeableModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // make certain that the correct type and metadata are inspected
            if (controllerContext.Controller.ViewData.Keys.ToList().IndexOf(bindingContext.ModelName) < 0)
                controllerContext.Controller.ViewData.Add(bindingContext.ModelName, bindingContext.ModelMetadata);
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}