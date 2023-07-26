using System;
namespace QuarterlySalesApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateOfHire { get; set; }
        public int ManagerId { get; set; }
    }
}

