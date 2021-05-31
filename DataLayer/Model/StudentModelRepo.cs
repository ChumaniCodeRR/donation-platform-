using DataLayer.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Model
{
    public class StudentModelRepo
    {
        private SqlConnection con;

        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["PPSDonationEntities"].ToString();
            con = new SqlConnection(constring);
        }

        public bool AddStudents(AspNetStudent smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddStudent_isp ", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", smodel.Firstname);
            cmd.Parameters.AddWithValue("@LastName", smodel.Lastname);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<AspNetStudent> GetStudents()
        {
            connection();
            List<AspNetStudent> studentList = new List<AspNetStudent>();

            SqlCommand cmd = new SqlCommand("GetStudentDetails_ssp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            //foreach(DataRow)


            return studentList;
        }
    }
}