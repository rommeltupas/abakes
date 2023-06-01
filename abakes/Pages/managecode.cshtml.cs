using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Text;

namespace abakes.Pages
{
    public class managecodeModel : PageModel
    {
        public List<code> codeList = new List<code>();
        public string userconfirm = "";
        public String connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");
            if(userconfirm != null)
            {

            }
            else
            {
                Response.Redirect("/index");

            }

            ReadCode();

        }

        public IActionResult OnGetDeactivate()
        {
            string id = Request.Query["id"];
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql2 = "update GenerateCode set status='false' where id='" + id + "'";

                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                command.ExecuteNonQuery();

                            }


                        }
                    }

                /*    String sql3 = "DELETE FROM GenerateCode WHERE id='" + id + "' AND status= 'false'" ;
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                command.ExecuteNonQuery();

                            }


                        }
                    } */
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return RedirectToPage("/managecode");

        }

        public IActionResult OnGetGenerate()
        {
            var rand = new Random();
            var code = new StringBuilder();
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var codeLength = 5;
            
            for (int i = 0; i < codeLength; i++)
            {
                int index = rand.Next(0, characters.Length);
                code.Append(characters[index]);
            }
            var maincode = code.ToString();

            try
            {
                DateTime expiration = DateTime.Now.AddDays(5);

                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql2 = "INSERT INTO GenerateCode " +
                                  "(code,status,date) VALUES " +
                                  "(@code,@status,@date);";

                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@code", maincode);
                        command.Parameters.AddWithValue("@status", "true");
                        command.Parameters.AddWithValue("@date", expiration);
                        
                        






                        command.ExecuteNonQuery();

                    }

                }

            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return Page();
            }


            return Redirect("/card?maincode=" + maincode);
        }

        
        public JsonResult OnGetExpiration()
        {
            ReadCode();
            
            DateTime current = DateTime.Now;
            string currentDate = current.ToString("MMM dd yyyy h:mm tt").Replace(" ","");
            string testdate = "";
            try
            {
               
                foreach (var i in codeList)
                {
                    
                    if (currentDate.Contains(i.dateexpiry.Replace(" ","")))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionProvider))
                        {
                            connection.Open();
                            String sql2 = "update GenerateCode set status='false' where id='" + i.id + "'";

                            using (SqlCommand command = new SqlCommand(sql2, connection))
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
                    }
                    else
                    {

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new { success = false });
            }

            return new JsonResult(new { success = true });
        }

        public JsonResult OnGetSession()
        {

            userconfirm = HttpContext.Session.GetString("user");
            if (userconfirm != null)
            {

            }
            else
            {
                Response.Redirect("/index");

            }

            Console.WriteLine("Hello");



            return new JsonResult(new { success = true });
        }



        public void ReadCode()
        {
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    String sql = "select * from GenerateCode order by id desc";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                code read = new code();


                                read.id = reader.GetInt32(0);
                                read.generatedCode = reader.GetString(1);
                                read.status = reader.GetString(2);
                                read.dateexpiry = reader.GetString(3);

                                codeList.Add(read);

                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }

    public class code
    {
        public int id;
        public string generatedCode;
        public string status;
        public string dateexpiry;

    }
}
