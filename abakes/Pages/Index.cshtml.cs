using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace abakes.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string userconfirm = "";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");


        }

        public IActionResult OnGetLogOut()
        {
            HttpContext.Session.Remove("user");
            userconfirm = HttpContext.Session.GetString("user");
            return RedirectToPage("/Index");
        }
    }


    public class UserInfo
    {
        public String id = "";
        public String username = "";
        public String password = "";
        public String secAnswer = "";
    }

    public class Products
    {
        public int pdID = 0;
        public string pdName = "";
        public string pdCategory = "";
        public int pdPrice = 0;
        public string pdDescription = "";
        public string pdImg = "";
        public string pdStatus = "";
    }

    public class OrderInfo
    {
        public int odID = 0;
        public string odName = "";
        public string odEmail = "";
        public string odPhone = "";
        public string odShapes = "";
        public string odTier = "";
        public string odFlavor = "";
        public string odSize = "";
        public string odInstruction = "";
        public string status = "";
    }
    public class Feedbacks
    {
        public int fbID = 0;
        public string fbName ="";
        public string fbRating ="";
        public string fbMessage = "";
        public string fbImg = "";
        public string status = "";

    }



}