using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CRUD_Operations.Areas.LOC_City.Models;
using CRUD_Operations.Areas.LOC_Country.Models;
using CRUD_Operations.Areas.LOC_State.Models;

namespace CRUD_Operations.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("/LOC_City/City")]
    public class CityController : Controller
    {
        public IConfiguration _configuration { get; set; }

        public CityController(IConfiguration config)
        {
            _configuration = config;
        }

        [Route("/CityList")]
        public IActionResult CityList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_SelectAll";



            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            return View(dt);


        }

        [Route("CitySelectByID")]

        public IActionResult CitySelectByID(int CityID)
        {


            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_SelectByPK";
            command.Parameters.AddWithValue("@CityID", CityID);


            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            return View(dt);
        }

        [Route("/AddEditForCity")]

        public IActionResult AddEditForCity(int CityID, int CountryID)
        {

            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_SelectByPK";
            command.Parameters.AddWithValue("@CityID", CityID);


            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            CityModel cityModel = new CityModel();

            foreach (DataRow dr in dt.Rows)
            {
                cityModel.CityID = int.Parse(dr["CityID"].ToString());
                cityModel.CityName = dr["CityName"].ToString();
                cityModel.CityCode = dr["CityCode"].ToString();
                cityModel.CountryID = int.Parse(dr["CountryID"].ToString());
                cityModel.StateID = int.Parse(dr["StateID"].ToString());
            }

            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_Country_SelectAll";



            SqlDataReader reader2 = command2.ExecuteReader();

            DataTable dt2 = new DataTable();
            dt2.Load(reader2);


            List<CountryModel> countryList = new List<CountryModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                CountryModel countryModel = new CountryModel();
                countryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                countryModel.CountryName = dr["CountryName"].ToString();
                countryList.Add(countryModel);
            }

            ViewBag.CountryList = countryList;
            ViewBag.CountryID = 3;




            ViewBag.CountryList = countryList;




            connection.Close();


            return View(cityModel);
        }


        [Route("/CityAddEditMethod")]

        public IActionResult CityAddEditMethod(CityModel data, int CityID = 0)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            if (CityID == 0)
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_City_Insert";

            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_City_Update";
                command.Parameters.AddWithValue("@CityID", CityID);

            }
            command.Parameters.AddWithValue("@CityName", data.CityName);
            command.Parameters.AddWithValue("@CountryID", data.CountryID);
            command.Parameters.AddWithValue("@StateID", data.StateID);
            command.Parameters.AddWithValue("@CityCode", data.CityCode);

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("CityList");
        }


        [Route("/CityDeleteRecord")]
        public IActionResult CityDeleteRecord(int CityID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_Delete";
            command.Parameters.AddWithValue("@CityID", CityID);



            command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("CityList");
        }

        [Route("/StatesForComboBox")]


        public dynamic StatesForComboBox(int CountryID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_StateByCountryID";
            command.Parameters.AddWithValue("CountryID", CountryID);



            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            List<StateModel> stateList = new List<StateModel>();
            foreach (DataRow dr in dt.Rows)
            {
                StateModel stateModel = new StateModel();
                stateModel.StateID = int.Parse(dr["StateID"].ToString());
                stateModel.StateName = dr["StateName"].ToString();
                stateList.Add(stateModel);
            }

            ViewBag.StateList = stateList;

            return Json(stateList);
        }

        [Route("/CountryFilter")]

        public dynamic CountryFilter(string CountryName)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_Country_Like";
            command.Parameters.AddWithValue("CountryName", CountryName);



            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            List<CountryModel> countryList = new List<CountryModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CountryModel countryModel = new CountryModel();
                countryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                countryModel.CountryName = dr["CountryName"].ToString();
                countryList.Add(countryModel);
            }

            ViewBag.CountryList = countryList;

            return Json(countryList);
        }

    }



}
