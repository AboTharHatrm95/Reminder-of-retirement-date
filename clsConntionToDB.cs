using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder_of_retirement_date
{
    public class clsConntionToDB
    {
        public static void GetRetirementDateByFullName(string FullName, ref int EmployeesID, ref DateTime Age, ref DateTime retirementDate,ref DateTime DateOfContract)
        {

            SqlConnection SqlConnection = new SqlConnection(clsSetting.ConnectionString);

            string Query = "Select * from RetirementDate Where FullName=@FullName";

            SqlCommand cmd = new SqlCommand(Query, SqlConnection);

            cmd.Parameters.AddWithValue("FullName", FullName);

            try
            {
                SqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    DateOfContract = (DateTime)reader["DateOfContract"];
                    retirementDate = (DateTime)reader["Retirementdate"];
                    EmployeesID = (int)reader["ID"];
                    Age = (DateTime)reader["dateofbirth"];

                }

            }
            catch
            {

            }
            finally
            {
                SqlConnection.Close();
            }

        }

        public static DataTable GetRetirementDateByDate(int Year)
        {
            DataTable dt = new DataTable();

            SqlConnection SqlConnection = new SqlConnection(clsSetting.ConnectionString);

            string Query = "SELECT * FROM RetirementDate  WHERE YEAR(RetirementDate) = @Year;";

            SqlCommand cmd = new SqlCommand(Query, SqlConnection);

            cmd.Parameters.AddWithValue("Year", Year);

            try
            {
                SqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        dt.Load(reader);

                    }


                }

            }
            catch
            {

            }
            finally
            {
                SqlConnection.Close();
            }
            return dt;
        }


    }
}
