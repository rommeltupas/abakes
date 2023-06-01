using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class ProductArchiveModel : PageModel
    {
        public List<Products> listProduct = new List<Products>();
        public List<UserInfo> userInfo = new List<UserInfo>();
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string userconfirm = "";
        public void GetProducts(string sortOrder)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from Product WHERE status ='false'  "; //getting the data based from the pdid variable

                    string search = Request.Query["search"];
                    if (!String.IsNullOrEmpty(search))
                    {
                        sql = "SELECT * FROM Product WHERE ProductName and status ='false' LIKE '%" + search + "%'";
                    }
                    switch (sortOrder)
                    {
                        case "Sort Name":
                            sql += "ORDER BY ProductName DESC";
                            break;
                        case "Sort Name2":
                            sql += "ORDER BY ProductName ASC";
                            break;
                        case "Sort Price":
                            sql += "ORDER BY ProductPrice DESC";
                            break;
                        case "Sort Price2":
                            sql += "ORDER BY ProductPrice ASC";
                            break;

                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Products pd = new Products();

                                pd.pdID = reader.GetFieldValue<int>(reader.GetOrdinal("ProductID"));
                                pd.pdName = reader.GetFieldValue<string>(reader.GetOrdinal("ProductName"));
                                pd.pdCategory = reader.GetFieldValue<string>(reader.GetOrdinal("ProductCategory"));
                                pd.pdPrice = reader.GetFieldValue<int>(reader.GetOrdinal("ProductPrice"));
                                pd.pdDescription = reader.GetFieldValue<string>(reader.GetOrdinal("ProductDesc"));
                                pd.pdImg = reader.GetFieldValue<string>(reader.GetOrdinal("ProductImg"));

                                listProduct.Add(pd);





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

        public IActionResult OnGetDelete()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "delete from Product where ProductID='" + id + "'"; //getting the data based from the pdid variable
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
            catch (Exception err)
            {

            }

            return Redirect("/ProductArchive");
        }

        public IActionResult OnGetArchive()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    String sql2 = "update Product set status='true' where ProductID='" + id + "'";
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
            catch (Exception err)
            {

            }

            return Redirect("/ProductArchive");
        }
        public void OnGet(string sortOrder)
        {
            userconfirm = HttpContext.Session.GetString("user");



            GetProducts(sortOrder);
        }
    }
}
