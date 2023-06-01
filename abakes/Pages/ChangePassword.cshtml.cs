using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class ChangePasswordModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public string userconfirm = "";
        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public void OnGet()
        {
          String id = Request.Query["Id"];
            userconfirm = HttpContext.Session.GetString("user");

            if (userconfirm != null)
            {

            }
            else
            {
                Response.Redirect("/index");
            }
        }
        public void OnPost()
        {
            String password = Request.Form["password"];
            int x = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql = "SELECT * FROM LoginSample WHERE username='Ahlia' and password='" + password + "' ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                x++;


                            }

                        }

                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql = "UPDATE LoginSample set password ='" + password + "' where username ='Ahlia' ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (x > 0)
                        {
                            errorMessage = "Please use a different password!";
                            
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                            TempData["AlertMessage"] = "Password Changed!";

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }
    }

  
}
