using Azure.Core.Pipeline;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Text;

namespace abakes.Pages
{
    public class ManageFeedbackModel : PageModel
    {
        public string userconfirm = "";
        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string errorMessage = "";
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");
            
            if(userconfirm != null)
            {
                Response.Redirect("/index");
            }
            else
            {

            }
        }


        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            string name = Request.Form["name"];
           
            string star = Request.Form["rating"];
            string secCode = Request.Form["securitycode"];
            string message = Request.Form["textmessage"];
            int checkSec = 0;
            try
            {
                //image upload
                if (file != null && file.Length > 0)
                {

                    using (SqlConnection connection = new SqlConnection(connectionProvider))
                    {
                        connection.Open();
                        string sql2 = "select * from GenerateCode where code='" + secCode + "' and status='true'";

                         //getting the data based from the pdid variable
                        using (SqlCommand command = new SqlCommand(sql2, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    checkSec++;
                                  




                                }
                            }
                        }
                        Console.WriteLine(checkSec);
                        if (checkSec > 0)
                        {

                            string fileName = Path.GetFileName(file.FileName);


                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "feedback", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }




                            string sql = "insert into feedback (name,rating,message,image,status) values (@name,@rating,@message,@image,'false')";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@name", name);
                                command.Parameters.AddWithValue("@rating", star);
                                command.Parameters.AddWithValue("@message", message);
                                command.Parameters.AddWithValue("@image", "/img/feedback/" + fileName);

                                command.ExecuteNonQuery();
                            }

                            string sql3 = "update GenerateCode set status='false' where code='" + secCode + "'";

                            using (SqlCommand command = new SqlCommand(sql3, connection))
                            {

                                command.ExecuteNonQuery();
                            }

                            String sql4 = "DELETE FROM GenerateCode WHERE status= 'false'"  ;
                            using (SqlCommand command = new SqlCommand(sql4, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        command.ExecuteNonQuery();

                                    }


                                }
                            }
                        }
                        else
                        {
                            errorMessage = "The code you used is invalid or has expired!";
                            return Page();
                        }



                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }

            return Redirect("/FeedbackCustomer");
        }
    }
}
