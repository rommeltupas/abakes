using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class ProductCustomerModel : PageModel
    {
        public List<Products> listProduct = new List<Products>();
        public List<UserInfo> userInfo = new List<UserInfo>();
        public int pdID = 0;
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string userconfirm = "";
        public string spageid;
        public double totalItems;
        public int pagecurrent;
       public int pageid;
        public int totals;



        public void GetProducts(int start, int totals, string sortOrder)
        {
            Console.WriteLine(start + " " + totals);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "SELECT TOP(" + totals + ") * FROM Product WHERE status='true' and ProductID NOT IN (SELECT TOP(" + (start - 1) + ") ProductID FROM Product)";
                    string search = Request.Query["search"];
                    if (!String.IsNullOrEmpty(search))
                    {
                        sql = "SELECT * FROM Product WHERE ProductName and status ='true' LIKE '%" + search + "%'";
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
                    //getting the data based from the pdid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                Products pd = new Products();
                                pd.pdID = reader.GetFieldValue<int>(reader.GetOrdinal("ProductID"));
                                pd.pdCategory = reader.GetFieldValue<string>(reader.GetOrdinal("ProductCategory"));
                                pd.pdName = reader.GetFieldValue<string>(reader.GetOrdinal("ProductName"));
                                pd.pdPrice = reader.GetFieldValue<int>(reader.GetOrdinal("ProductPrice"));
                                pd.pdDescription = reader.GetFieldValue<string>(reader.GetOrdinal("ProductDesc"));
                                pd.pdImg = reader.GetFieldValue<string>(reader.GetOrdinal("ProductImg"));
                                pd.pdStatus = reader.GetFieldValue<string>(reader.GetOrdinal("status"));

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
        public void OnGet(string sortOrder)
        {

            spageid = Request.Query["page"];
            pageid = int.Parse(spageid);
             totals = 9;
            totalItems = 0.0;
            if (pageid == 1) { }
            
            else
            {
                pageid = pageid - 1;
                pageid = pageid * totals + 1;
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from Product WHERE status ='true'";
                    //getting the data based from the pdid variable
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {


                                totalItems++;




                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error Reading Products: " + e.ToString());

            }

            pagecurrent = int.Parse(spageid);

            int varId = 0;
            userconfirm = HttpContext.Session.GetString("user");
            GetProducts(pageid, totals, sortOrder);
        }
    }
}
