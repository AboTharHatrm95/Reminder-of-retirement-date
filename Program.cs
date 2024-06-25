using System;
using System.Collections.Generic;
using System.Data;


namespace Reminder_of_retirement_date
{

    public class ReminderEvent : EventArgs
    {
        
        public string FullName { get; }
        public int EmployeesID { get; }
        public DateTime dateofbirth { get; }
        public DateTime Retirementdate { get; }
        private DateTime DateOfContract { get; }
        public float YearsOfService { get; }

        public ReminderEvent(DataRow dataRow)
        {
            EmployeesID = (int)dataRow["ID"];

            FullName = dataRow["FullName"].ToString();

            dateofbirth = (DateTime)dataRow["dateofbirth"];

            Retirementdate = (DateTime)dataRow["Retirementdate"];

            YearsOfService = Retirementdate.Year -  Convert.ToDateTime(dataRow["DateOfContract"]).Year;
        }

        public ReminderEvent(string FullName,int employeesID, DateTime age, DateTime retirementdate, DateTime DateOfContract)
        {
            this.FullName = FullName;
            EmployeesID = employeesID;
            dateofbirth = age;
            Retirementdate = retirementdate;
            this.DateOfContract = DateOfContract;

            YearsOfService = Retirementdate.Year - DateOfContract.Year;

        }
    }

    public class Retirement
    {
        public event EventHandler<ReminderEvent> RetirementCloess;

        private DateTime TimeNow = DateTime.Now.Date;


        public void CheckifyourRetirementDateisNear(string FullName)
        {
            DateTime Retirementdate=DateTime.Now ,age= DateTime.Now , DateOfContract=DateTime.Now; int EmployeesID = -1;
            
            clsConntionToDB.GetRetirementDateByFullName(FullName, ref EmployeesID, ref age, ref Retirementdate,ref DateOfContract);


            if (Retirementdate == TimeNow) 
            {
                Console.WriteLine("This Employees Will Retirement");

                RetirementCloess(this,new ReminderEvent(FullName,EmployeesID,age, Retirementdate,DateOfContract));
                return;

            }
            Console.WriteLine("The retirement date for this employee is not now,\n Actual date: " + Retirementdate);

            
        }

        //Res Event If There Any Data Using Year
        public void CheckifyourRetirementDateisNear(int Year)
        {
            if (Year < 1000 || Year > 9999) 
            {
                Console.WriteLine("Invalid year format. Please enter a Correct year. Range 1000 - 9999 ");
                return;
            }


            DataTable dt = clsConntionToDB.GetRetirementDateByDate(Year);

            bool hasRetirementThisYear = false;


            List<ReminderEvent> retirementEvents = new List<ReminderEvent>();



            foreach (DataRow dr in dt.Rows)
            {
                DateTime retirementDate = Convert.ToDateTime(dr["Retirementdate"]);

                if (retirementDate.Year == Year)
                {
                    hasRetirementThisYear = true;

                    var Ret = new ReminderEvent(dr);

                    retirementEvents.Add(Ret);

                  
                }
            }


            if (hasRetirementThisYear)
            {
                Console.WriteLine("These Employees Will Retire In This Year: " + Year);

                foreach (var retirementEvent in retirementEvents)
                {
                    // You can handle each reminderEvent as needed
                    RetirementCloess(this, retirementEvent);
                }

            }
            else
            {
                Console.WriteLine("No employees will retire in the year " + Year);

            }

            


        }


    }

    public class RetirementDepartment
    {
        public void Subscrabir(Retirement retirement)
        {
            retirement.RetirementCloess += DisplyResult;
        }

        public void UnSubscrabir(Retirement retirement)
        {
            retirement.RetirementCloess -= DisplyResult;
        }

        public void DisplyResult(object Sender, ReminderEvent e)
        {
            Console.WriteLine("\n-------------Employee details----------------");
            Console.WriteLine("Employees ID: " + e.EmployeesID);
            Console.WriteLine("Employees Name :"+ e.FullName);
            Console.WriteLine("date of birth: " + e.dateofbirth);
            Console.WriteLine("Retirement date: " + e.Retirementdate);
            Console.WriteLine("Years of service : " + e.YearsOfService);
            Console.WriteLine("----------------------------------------------");


        }

    }


    internal class Program
    {

        static void Main(string[] args)
        {
            //Serves
            Retirement retirement = new Retirement();

            

            RetirementDepartment Dep= new RetirementDepartment();

            //New SubSCRIBAR
            Dep.Subscrabir(retirement);


            // Res Event If This Employees Retiremented Using Full Name
           // retirement.CheckifyourRetirementDateisNear("Hateem Shanoon");


            // Res Event If There Any Emploees Retirement In This Year
           // retirement.CheckifyourRetirementDateisNear(2024);

            Console.ReadKey();


        }
    }
}
