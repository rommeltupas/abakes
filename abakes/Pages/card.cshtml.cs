using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace abakes.Pages
{
    public class cardModel : PageModel
    {
        public string code = "";
        public void OnGet()
        {
            string userconfirm = HttpContext.Session.GetString("user");

            if(userconfirm != null)
            {

            }
            else
            {
                Response.Redirect("/index");
            }
            code = Request.Query["maincode"];
        }
    }
}
