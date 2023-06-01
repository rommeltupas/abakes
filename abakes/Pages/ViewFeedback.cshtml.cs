using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class ViewFeedbackModel : PageModel
    {
        public List<Feedbacks>listFeedback = new List<Feedbacks>();
        public string userconfirm = "";
        public String errorMessage = "";
        public String successMessage = "";
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public int fbID = 0;
        public string fbName = "";
        public string fbRating = "";
        public String fbMessage = "";
        public String fbImg = "";

        public void GetFeedbacks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from Feedback"; //getting the data based from the pdid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Feedbacks fb = new Feedbacks();
                                fb.fbID = reader.GetFieldValue<int>(reader.GetOrdinal("Id"));
                                fb.fbName = reader.GetFieldValue<string>(reader.GetOrdinal("FeedbackName"));
                                fb.fbRating = reader.GetString(reader.GetOrdinal("FeedbackRating"));
                                fb.fbMessage = reader.GetFieldValue<string>(reader.GetOrdinal("FeedbackMessage"));

                                listFeedback.Add(fb);

                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error Reading Products: " + e.ToString());

            }
        }
        public void OnGet()
        {
            GetFeedbacks();
            String id = Request.Query["id"];
            userconfirm = HttpContext.Session.GetString("user");
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM feedback WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fbID = reader.GetInt32(0);
                                fbName = reader.GetString(1);
                                fbRating = reader.GetString(2);
                                fbMessage = reader.GetString(3);
                                fbImg = reader.GetString(4);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
