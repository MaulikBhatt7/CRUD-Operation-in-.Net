using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

using CRUD_Operations_Using_Area.Areas.Branch.Models;

namespace CRUD_Operations_Using_Area.Areas.Branch.Controllers
{
    [Area("Branch")]
    
    public class Branch : Controller
    {
        public readonly IConfiguration _Configuration;
        public Branch(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [Route("/BranchList")]
        public IActionResult BranchList()
        {

            SqlConnection sqlConnection = new SqlConnection(this._Configuration.GetConnectionString("connstr"));

            sqlConnection.Open();

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Branch_SelectAll";

            DataTable dt = new DataTable();

            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);

            return View(dt);
        }

        [Route("/AddEditForBranch")]

        public IActionResult AddEditForBranch(int BranchID)
        {
            SqlConnection connection = new SqlConnection(this._Configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_SelectByPK";
            command.Parameters.AddWithValue("@BranchID", BranchID);

            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            BranchModel branchModel = new BranchModel();

            foreach (DataRow dr in dt.Rows)
            {
                branchModel.BranchID = int.Parse(dr["BranchID"].ToString());
                branchModel.BranchName = dr["BranchName"].ToString();
                branchModel.BranchCode = dr["BranchCode"].ToString();
            }

            connection.Close();

            return View(branchModel);
        }


        [Route("/AddEditMethodForBranch")]

        public IActionResult AddEditMethodForBranch(BranchModel data, int? BranchID)
        {
            SqlConnection connection = new SqlConnection(this._Configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            if (BranchID == null)
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Branch_Insert";
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Branch_UpdateByPK";
                command.Parameters.AddWithValue("@BranchID", BranchID);
            }
            command.Parameters.AddWithValue("@BranchName", data.BranchName);
            command.Parameters.AddWithValue("@BranchCode", data.BranchCode);

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("BranchList");
        }

        [Route("/DeleteFromBranch")]

        public IActionResult DeleteFromBranch(int BranchID)
        {

            SqlConnection connection = new SqlConnection(this._Configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_DeleteByPK";
            command.Parameters.AddWithValue("@BranchID", BranchID);

            command.ExecuteNonQuery();

            connection.Close();


            return RedirectToAction("BranchList");
        }

    }
}
