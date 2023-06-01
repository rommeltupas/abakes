using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace abakes.Pages
{
    public class ContactModel : PageModel
    {
        public List<UserInfo> listUser = new List<UserInfo>();
        public List<Products> listProduct = new List<Products>();
        public List<OrderInfo> listOrder = new List<OrderInfo>();
        public string userconfirm = "";
        public String errorMessage = "";
        public String successMessage = "";
        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";


        public void OnPost()
        {
            
            string Name = Request.Form["fullname"];
            string Email = Request.Form["email"];
            string Phone = Request.Form["phone"];
            string Shape = Request.Form["cake-shape"];
            string Tier = Request.Form["cake-tier"];
            string Type = Request.Form["cake_type"];
            string Size = Request.Form["cake_size"];
            string Instruction = Request.Form["special_instructions"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionProvider))
                {
                    connection.Open();
                    string sql = "insert into OrderForm (Name, Email, Phone, Shapes, Tier, CakeFlavors, CakeSizes, CakeInstruction, status)" +
                        "VALUES(@Name, @Email, @Phone, @Shapes, @Tier, @CakeFlavors, @CakeSizes, @CakeInstruction, 'true');";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        command.Parameters.AddWithValue("@Shapes", Shape);
                        command.Parameters.AddWithValue("@Tier", Tier);
                        command.Parameters.AddWithValue("@CakeFlavors", Type);
                        command.Parameters.AddWithValue("@CakeSizes", Size);
                        command.Parameters.AddWithValue("@CakeInstruction", Instruction);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");
            
        }
    }
}
