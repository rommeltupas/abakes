using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace abakes.Pages
{
    public class OrderListModel : PageModel
    {
        public List<OrderInfo> listOrder = new List<OrderInfo>();
        public List<UserInfo> userInfo = new List<UserInfo>();
        public string connectionString = "Data Source=LAPTOP-P14SL805;Initial Catalog=Abakes;Integrated Security=True";
        public string userconfirm = "";

        public void GetOrders(string sortOrder)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) //static
                {
                    connection.Open();
                    string sql = "select * from OrderForm  "; //getting the data based from the odid variable
                    string search = Request.Query["search"];
                    if (!String.IsNullOrEmpty(search))
                    {
                        sql = "SELECT * FROM OrderForm WHERE status ='true' AND Name LIKE '%" + search + "%' ";
                    }

                    switch (sortOrder)
                    {
                        case "Sort Name":
                            sql += "ORDER BY Name DESC";
                            break;
                        case "Sort Name2":
                            sql += "ORDER BY Name ASC";
                            break;
                        case "Sort Flavor":
                            sql += "ORDER BY CakeFlavors DESC";
                            break;
                        case "Sort Flavor2":
                            sql += "ORDER BY CakeFlavors ASC";
                            break;

                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderInfo od = new OrderInfo();
                               

                                od.odID = reader.GetFieldValue<int>(reader.GetOrdinal("Id"));
                                od.odName = reader.GetFieldValue<string>(reader.GetOrdinal("Name"));
                                od.odEmail = reader.GetFieldValue<string>(reader.GetOrdinal("Email"));
                                od.odPhone = reader.GetFieldValue<string>(reader.GetOrdinal("Phone"));
                                od.odShapes = reader.GetFieldValue<string>(reader.GetOrdinal("Shapes"));
                                od.odTier = reader.GetFieldValue<string>(reader.GetOrdinal("Tier"));
                                od.odFlavor = reader.GetFieldValue<string>(reader.GetOrdinal("CakeFlavors"));
                                od.odSize = reader.GetFieldValue<string>(reader.GetOrdinal("CakeSizes"));
                                od.odInstruction = reader.GetFieldValue<string>(reader.GetOrdinal("CakeInstruction"));
                                od.status = reader.GetFieldValue<string>(reader.GetOrdinal("status"));
                                listOrder.Add(od);





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

        public IActionResult OnGetDeactivate()
        {
            string id = Request.Query["Id"];
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql2 = "update OrderForm set status='false' where Id='" + id + "'";

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

            return RedirectToPage("/OrderList");
        }

        public IActionResult OnGetDelete()
        {
            string id = Request.Query["Id"];
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql2 = "DELETE FROM OrderForm WHERE Id='" + id + "'";

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

            return RedirectToPage("/OrderList");
        }
        public void OnGet(string sortOrder)
        {
            GetOrders(sortOrder);
        }
    }
}
