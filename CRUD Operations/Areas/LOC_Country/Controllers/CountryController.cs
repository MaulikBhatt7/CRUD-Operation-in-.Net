using CRUD_Operations.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CRUD_Operations.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("/LOC_Country/Country")]
    public class CountryController : Controller
    {
        
        public readonly IConfiguration _configuration;

        public CountryController(IConfiguration config)
        {
            _configuration= config;
        }

        [Route("/CountryList")]
        
        public IActionResult CountryList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectAll";


          
            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            return View(dt);
        }

        [Route("/DeleteRecord")]

        public IActionResult DeleteRecord(int CountryID)
        {

            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_Delete";
            command.Parameters.AddWithValue("@CountryID", CountryID);

            command.ExecuteNonQuery();

            connection.Close();


            return RedirectToAction("CountryList");
        }

        [Route("/AddEdit")]

        public IActionResult AddEdit(int CountryID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectByPK";
            command.Parameters.AddWithValue("@CountryID", CountryID);

            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            CountryModel countryModel = new CountryModel();

            foreach(DataRow dr in dt.Rows)
            {
                countryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                countryModel.CountryName = dr["CountryName"].ToString();
                countryModel.CountryCode = dr["CountryCode"].ToString();
            }

            connection.Close();

            return View(countryModel);
        }

        [Route("/AddEditMethod")]

        public IActionResult AddEditMethod(CountryModel data,int? CountryID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            if (CountryID == null)
            { 
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Country_Insert";
                command.Parameters.AddWithValue("@CountryName", data.CountryName);
                command.Parameters.AddWithValue("@CountryCode", data.CountryCode);
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Country_Update";
                command.Parameters.AddWithValue("@CountryID", CountryID);
                command.Parameters.AddWithValue("@CountryName", data.CountryName);
                command.Parameters.AddWithValue("@CountryCode", data.CountryCode);
            }

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("CountryList");
        }
    }
}
