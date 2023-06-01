using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace abakes.Pages
{
    public class ManageProductModel : PageModel
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

        public IActionResult OnGetDelete()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "delete * from Product where ProductID='" + id + "'"; //getting the data based from the pdid variable
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
            }catch(Exception err)
            {

            }

                    return Redirect("/ProductList");
        }

            [HttpPost]
            public async Task<IActionResult> OnPostAsync(IFormFile file)
            {
            string pdid = Request.Form["id"];
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
                getProductInfo();
                int counter = GetProducts(pdID+"", pdname);

                    if (counter == 0)
                    {
                        try
                        {


                            using (SqlConnection connection = new SqlConnection(connectionProvider))
                            {
                                connection.Open();
                                String sql2 = "update Product set ProductCategory='" + category + "', ProductName='" + pdname + "', ProductPrice='" + price + "', ProductDesc='" + desc + "', ProductImg='/img/menu/" + fileName + "' where ProductID='" + pdid + "'"; 

                                using (SqlCommand command = new SqlCommand(sql2, connection))
                                {
                                    



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
                    getProductInfo();
                        return Page();
                    }


                }

                return Redirect("/ProductList");
            }



        public void getProductInfo()
        {
            string id = Request.Query["id"];
            pdID = int.Parse(id);
            Console.WriteLine(id);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from Product where ProductID='" + id + "'"; //getting the data based from the pdid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                pdName = reader.GetString(reader.GetOrdinal("ProductName"));
                                pdCategory = reader.GetString(reader.GetOrdinal("ProductCategory"));
                                pdPrice = reader.GetInt32(reader.GetOrdinal("ProductPrice"));
                                pdDescription = reader.GetString(reader.GetOrdinal("ProductDesc"));
                                pdImg = reader.GetString(reader.GetOrdinal("ProductImg"));
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
            public int GetProducts(string id, string pdname)
            {

            int count = 0;  
            string name = "";
                
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString)) //static
                    {
                        connection.Open();
                        string sql = "select * from Product where ProductID='" + id + "'"; //getting the data based from the pdid variable
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                name = reader.GetString(2);

                                }
                            }

                      
                       
                        
                    }
                    string sql2 = "select * from Product where ProductName !='" + name + "'";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name = reader.GetString(2);

                                bool isEqual = name.Equals(pdname, StringComparison.OrdinalIgnoreCase);

                                if (isEqual)
                                {
                                    count++;
                                }
                                else
                                {

                                }
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
               
            getProductInfo();

            
            }
        }
    }

