using Data_Annotation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace Data_Annotation.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration;

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SQLView() {
            SqlConnection sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("default"));
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlCommand.CommandText = "PR_Country_SelectAll";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(sqlDataReader);
            return View(dt);
        }

        public IActionResult SelectByID(int CountryID)
        {
            SqlConnection sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("default"));
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlCommand.CommandText = "PR_Country_SelectByPK";
            sqlCommand.Parameters.AddWithValue("CountryID", CountryID);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(sqlDataReader);
            return View(dt);
        }

        public IActionResult DeleteByID(int CountryID)
        {
            SqlConnection sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("default"));
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Country_Delete";
            sqlCommand.Parameters.AddWithValue("CountryID", CountryID);

            sqlCommand.ExecuteNonQuery();
            
            return RedirectToAction("SQLView");
        }

        public IActionResult AddEdit(int CountryID=0)
        {
            SqlConnection sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("default"));
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlCommand.CommandText = "PR_Country_SelectByPK";
            sqlCommand.Parameters.AddWithValue("CountryID", CountryID);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            CountryModel data = new CountryModel();

            foreach(DataRow dr in dt.Rows)
            {
                data.CountryId = int.Parse(dr["CountryID"].ToString());
                data.CountryName = dr["CountryName"].ToString();
                data.CountryCode = dr["CountryCode"].ToString();
            }  
          
            return View(data);
        }

        public IActionResult AddEditMethod(CountryModel data,int CountryID=0)
        {
            SqlConnection sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("default"));
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType= System.Data.CommandType.StoredProcedure;

            if (CountryID==0)
            {
                sqlCommand.CommandText = "PR_Country_Insert";
                sqlCommand.Parameters.AddWithValue("CountryName", data.CountryName);
                sqlCommand.Parameters.AddWithValue("CountryCode", data.CountryCode);
                    
                sqlCommand.ExecuteNonQuery();
            }
            else
            {

                sqlCommand.CommandText = "PR_Country_Update";
                sqlCommand.Parameters.AddWithValue("CountryID", CountryID);
                sqlCommand.Parameters.AddWithValue("CountryName", data.CountryName);
                sqlCommand.Parameters.AddWithValue("CountryCode", data.CountryCode);

                sqlCommand.ExecuteNonQuery();

            }
            return RedirectToAction("SQLView");
            

        }

        
    }
}