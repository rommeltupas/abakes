using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class FeedbackCustomerModel : PageModel
    {
        public List<Feedbacks> listFeedback = new List<Feedbacks>();
        public List<UserInfo> userInfo = new List<UserInfo>();
        public int fbID = 0;
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string userconfirm = "";
        public void GetFeedbacks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from feedback where status='true'"; //getting the data based from the fbid variable
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
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");
            GetFeedbacks();
        }
    }
}
