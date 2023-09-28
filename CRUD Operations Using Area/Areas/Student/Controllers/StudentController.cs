using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CRUD_Operations.Areas.LOC_City.Models;
using CRUD_Operations.Areas.LOC_Country.Models;
using CRUD_Operations_Using_Area.Areas.Student.Models;
using System.Windows.Input;

namespace CRUD_Operations_Using_Area.Areas.Student.Controllers
{
    [Area("Student")]

    public class StudentController : Controller
    {
        public IConfiguration _configuration { get; set; }
        public StudentController(IConfiguration config) {
            _configuration = config;
        }

        [Route("/StudentList")]
        public IActionResult StudentList()
        {


            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[PR_Student_SelectAll]";



            SqlDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            return View(dt);
        }

        [Route("/DeleteFromStudent")]

        public IActionResult DeleteFromStudent(int StudentID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_DeleteByPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);


            command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("StudentList");
        }

        [Route("/AddEditForStudent")]

        public IActionResult AddEditForStudent(int StudentID)
        {

            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_SelectByPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);



            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            connection.Close();

            StudentModel studentModel = new StudentModel();

            foreach(DataRow dr in dt.Rows)
            {
                studentModel.StudentID = int.Parse(dr["StudentID"].ToString());
                studentModel.StudentName = dr["StudentName"].ToString();
                studentModel.Email = dr["Email"].ToString();
                studentModel.Address = dr["Address"].ToString();
                studentModel.Password = dr["Password"].ToString();
                studentModel.MobileNoStudent = dr["MobileNoStudent"].ToString();
                studentModel.MobileNoFather = dr["MobileNoFather"].ToString();
                studentModel.Age = int.Parse(dr["Age"].ToString());
                studentModel.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString()).ToString("d");
                studentModel.CityID = int.Parse(dr["CityID"].ToString());
                studentModel.BranchID = int.Parse(dr["BranchID"].ToString());
            }
            return View(studentModel);
        }

        [Route("/SaveStudent")]

        public IActionResult SaveStudent(StudentModel studentModel,int StudentID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();

            if (StudentID == 0)
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Student_Insert";
                

            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Student_UpdateByPK";
                command.Parameters.AddWithValue("@StudentID", StudentID);
            }
           
            command.Parameters.AddWithValue("@StudentName", studentModel.StudentName);
            command.Parameters.AddWithValue("@IsActive", studentModel.IsActive);
            command.Parameters.AddWithValue("@Email", studentModel.Email);
            command.Parameters.AddWithValue("@Gender", studentModel.Gender);
            command.Parameters.AddWithValue("@Address", studentModel.Address);
            command.Parameters.AddWithValue("@MobileNoFather", studentModel.MobileNoFather);
            command.Parameters.AddWithValue("@MobileNoStudent", studentModel.MobileNoStudent);
            command.Parameters.AddWithValue("@Age", studentModel.Age);
            command.Parameters.AddWithValue("@BirthDate", studentModel.BirthDate);
            command.Parameters.AddWithValue("@Password", studentModel.Password);
            command.Parameters.AddWithValue("@BranchID", studentModel.BranchID);
            command.Parameters.AddWithValue("@CityID", studentModel.CityID);




            command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("StudentList");
        }

        [Route("/DeleteStudent")]
        public IActionResult DeleteRecord(int StudentID)
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("connstr"));

            //connection open
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_DeleteByPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);

            command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("StudentList");
        }

     
       
    }
}
