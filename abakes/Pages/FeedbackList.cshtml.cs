using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class FeedbackListModel : PageModel
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
                    string sql = "select * from feedback order by id desc"; //getting the data based from the fbid variable
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


        public IActionResult OnGetRemove()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "delete from feedback where id='" + id + "'"; //getting the data based from the fbid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
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
                Console.WriteLine("Error Removing Feedbacks: " + e.ToString());

            }

            return Redirect("/FeedbackList");
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
