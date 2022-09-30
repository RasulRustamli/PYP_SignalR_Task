using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserRegistrationMvc.CustomActionFilters
{
    public class CheckAuthorize : ActionFilterAttribute
    {
        public string[] Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag = true;
            foreach (var item in Roles)
            {
                var role = filterContext.HttpContext.Session.GetString("role");
                if (item != role) flag = false;
                else flag = true;
            }
            if (flag == false) filterContext.Result = new RedirectResult("/Admin/RoleUsers/Index");
        }

    }
}
