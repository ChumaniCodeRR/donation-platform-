namespace PayFast.AspNet
{
    using System;
  
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ValueProviders;
    public class PayFastNotifyModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                var controllerContext = actionContext.ControllerContext;
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                var model = new PayFastNotify();
                bindingContext.Model = model;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }
        
    }
}
