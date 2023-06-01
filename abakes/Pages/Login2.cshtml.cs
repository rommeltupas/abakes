using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace abakes.Pages
{
    public class Login2Model : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string userconfirm = "";
        public void OnGet()
        {
            String id = Request.Query["Id"];
            userconfirm = HttpContext.Session.GetString("user");

            if (userconfirm != null)
            {
                HttpContext.Session.Remove("user");

            }
            else
            {

            }


        }

        public void OnPost()
        {
            String inputString = Request.Form["inputString"];
            String password = Request.Form["password"];
            
            string newpass = Request.Form["cpassword"];
            int x = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql = "SELECT * FROM LoginSample WHERE username='Ahlia' and secAnswer='" + inputString + "'";
                    using(SqlCommand command = new SqlCommand(sql, connection))
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
                    String sql = "UPDATE LoginSample set password ='" + password + "' where username ='Ahlia' and secAnswer='" + inputString + "' ";
                    using(SqlCommand command = new SqlCommand( sql, connection))
                    {
                        if (x > 0 && password.Equals(newpass))
                        {
                            command.ExecuteNonQuery();
                            TempData["AlertMessage"] = "Password Changed!";

                            

                        }
                        else
                        {
                            Response.Redirect("/Login");

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
