using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class FeedbackVerifyModel : PageModel
    {
        public List<Feedbacks> listFeedback = new List<Feedbacks>();
        public List<UserInfo> userInfo = new List<UserInfo>();
        public string userconfirm = "";

        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";

        public void GetFeedbacks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from feedback WHERE status='false' order by id desc"; //getting the data based from the fbid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                Feedbacks fb = new Feedbacks();
                                fb.fbID = reader.GetFieldValue<int>(reader.GetOrdinal("id"));
                                fb.fbName = reader.GetFieldValue<string>(reader.GetOrdinal("name"));
                                fb.fbRating = reader.GetString(reader.GetOrdinal("rating"));
                                fb.fbMessage = reader.GetFieldValue<string>(reader.GetOrdinal("message"));
                                fb.fbImg = reader.GetFieldValue<string>(reader.GetOrdinal("image"));
                                fb.status = reader.GetFieldValue<string>(reader.GetOrdinal("status"));

                                listFeedback.Add(fb);





                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error Reading Feedbacks: " + e.ToString());

            }
        }
        public IActionResult OnGetVerify()
        {
            string id = Request.Query["id"];
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql2 = "update feedback set status='true' where id='" + id + "'";

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
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return RedirectToPage("/FeedbackVerify");

        }
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

            GetFeedbacks();
        }
    }
}
