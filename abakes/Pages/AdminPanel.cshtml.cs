using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace abakes.Pages
{
    public class AdminPanelModel : PageModel
    {
        public string userconfirm = "";
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");

            if (userconfirm != null)
            {

            }
            else
            {
                Response.Redirect("/index");
            }
        }

    }
}
