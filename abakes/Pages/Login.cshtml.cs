using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string username { get; set; }
        public string password { get; set; }
        public string userconfirm = "";

        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string errorMessages = "";
        public string pass = "";

        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");

            if (userconfirm != null)
            {
                HttpContext.Session.Remove("user");

            }
            else
            {

            }
        }

        public IActionResult OnPost()
        {
            username = Request.Form["username"];
            password = Request.Form["password"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql = "SELECT * FROM LoginSample WHERE username='" + username + "'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@x", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                username = reader.GetString(1);
                                pass = reader.GetString(2);

                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }

            if (!password.Equals(pass))
            {
                errorMessages = "Invalid email or Password!";
                return Page();

            }
            else
            {

                HttpContext.Session.SetString("user", username);
                userconfirm = HttpContext.Session.GetString("user");

                return RedirectToPage("/AdminPanel");
            }

        }

    }
}

