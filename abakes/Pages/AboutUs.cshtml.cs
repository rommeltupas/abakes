using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace abakes.Pages
{
    public class AboutUsModel : PageModel
    {
        public string userconfirm = "";
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");


        }
    }
}
