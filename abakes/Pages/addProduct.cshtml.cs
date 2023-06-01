using Azure.Core.Pipeline;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Text;

namespace abakes.Pages
{


    public class addProductModel : PageModel
    {


        public List<UserInfo> listUser = new List<UserInfo>();
        public List<Products> listProduct = new List<Products>();
        
        public string userconfirm = "";
        public String errorMessage = "";
        public String successMessage = "";
        public string connectionProvider = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public int pdID = 0;
        public string pdName = "";
        public string pdCategory = "";
        public int pdPrice = 0;
        public string pdDescription = "";
        public string pdImg = "";
    

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            string pdname = Request.Form["name"];
            string category = Request.Form["category"];
            string price = Request.Form["price"];
            string desc = Request.Form["description"];

            if (file != null && file.Length > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "menu", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                int counter = GetProducts(pdname);

                if (counter == 0)
                {
                    try
                    {


                        using (SqlConnection connection = new SqlConnection(connectionProvider))
                        {
                            connection.Open();
                            String sql2 = "INSERT INTO Product " +
                                          "(ProductCategory,ProductName,ProductPrice,ProductDesc,ProductImg,status) VALUES " +
                                          "(@ProductCategory,@ProductName,@ProductPrice,@ProductDesc,@ProductImg,'true');";

                            using (SqlCommand command = new SqlCommand(sql2, connection))
                            {
                                command.Parameters.AddWithValue("@ProductCategory", category);
                                command.Parameters.AddWithValue("@ProductName", pdname);
                                command.Parameters.AddWithValue("@ProductPrice", price);
                                command.Parameters.AddWithValue("@ProductDesc", desc);
                                command.Parameters.AddWithValue("@ProductImg", "/img/menu/" + fileName);



                                command.ExecuteNonQuery();

                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return Page();
                    }
                }
                else
                {
                    errorMessage = "This Product is Already Added!";
                    return Page();
                }


            }

            return Redirect("/ProductList");
        }




        public int GetProducts(string pdname)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from Product where ProductName='" + pdname +"'"; //getting the data based from the pdid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                count++;
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error Reading Products: " + e.ToString());

            }
            return count;
        }
        public void OnGet()
        {
            userconfirm = HttpContext.Session.GetString("user");

            
           /* String id = Request.Query["id"];*/

            /*try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product WHERE ProductID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pdID = reader.GetInt32(0);
                                pdName = reader.GetString(2);
                                pdCategory = reader.GetString(1);
                                pdPrice = reader.GetInt32(3);
                                pdDescription = reader.GetString(4);
                                pdImg = reader.GetString(5);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }*/
        }
    }
}

