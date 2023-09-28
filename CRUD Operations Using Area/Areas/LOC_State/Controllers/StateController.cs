using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CRUD_Operations.Areas.LOC_State.Models;
using CRUD_Operations.Areas.LOC_Country.Models;

namespace CRUD_Operations.Areas.LOC_State.Controllers { 
    [Area("LOC_State")]
    [Route("/LOC_State/State")]
    public class StateController : Controller
    {
        public IConfiguration _configuration { get; set; }

        public StateController(IConfiguration config)
        {
            _configuration = config;
        }
        [Route("/StateList")]
        public IActionResult StateList(int? CountryID=0,string stateCode="")
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_SelectStateByCountryAndStateCode";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            command.Parameters.AddWithValue("@StateCode", stateCode);


            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_Country_SelectAll";



            SqlDataReader reader2 = command2.ExecuteReader();

            DataTable dt2 = new DataTable();
            dt2.Load(reader2);


            List<CountryModel> li = new List<CountryModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                CountryModel countryModel = new CountryModel();
                countryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                countryModel.CountryName = dr["CountryName"].ToString();
                li.Add(countryModel);
            }
            ViewBag.CountryList = li;


            connection.Close();

            return View(dt);
        }


        public IActionResult SelectByID(int StateID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_State_SelectByPK";
            command.Parameters.AddWithValue("@StateID", StateID);


            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            return View(dt);
        }

        [Route("/StateAddEdit")]

        public IActionResult StateAddEdit(int StateID)
        {

            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_State_SelectByPK";
            command.Parameters.AddWithValue("@StateID", StateID);


            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

           

            StateModel stateModel = new StateModel();

            foreach(DataRow dr in dt.Rows)
            {
                stateModel.StateID = int.Parse(dr["StateID"].ToString());
                stateModel.StateName = dr["StateName"].ToString();
                stateModel.StateCode = dr["StateCode"].ToString();
                stateModel.CountryID = int.Parse(dr["CountryID"].ToString());
            }

            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_Country_SelectAll";
           


            SqlDataReader reader2 = command2.ExecuteReader();

            DataTable dt2 = new DataTable();
            dt2.Load(reader2);

            
            List<CountryModel> li = new List<CountryModel>();
            foreach(DataRow dr in dt2.Rows)
            {
                CountryModel countryModel = new CountryModel();
                countryModel.CountryID = int.Parse(dr["CountryID"].ToString());
                countryModel.CountryName = dr["CountryName"].ToString();
                li.Add(countryModel);
            }
            ViewBag.CountryList = li;


            connection.Close();


            return View(stateModel);
        }

        [Route("/StateAddEditMethod")]

        public IActionResult StateAddEditMethod(StateModel data,int StateID=0)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            if (StateID == 0)
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_State_Insert";
                
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_State_Update";
                command.Parameters.AddWithValue("@StateID", StateID);
                
            }
            command.Parameters.AddWithValue("@StateName", data.StateName);
            command.Parameters.AddWithValue("@CountryID", data.CountryID);
            command.Parameters.AddWithValue("@StateCode", data.StateCode);

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("StateList");
        }

        [Route("/StateDeleteRecord")]
        public IActionResult StateDeleteRecord(int StateID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_State_Delete";
            command.Parameters.AddWithValue("@StateID", StateID);



          command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("StateList");
        }
    }
}
