using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities;

public class Employee
{
    public int EmployeeID { get; set; }         // SAP tomonidan generate qilinadi
    public string FirstName { get; set; }       // majburiy
    public string LastName { get; set; }        // majburiy
    public string JobTitle { get; set; }        // majburiy
    public int? Department { get; set; }      // default -2
    public int? Branch { get; set; }          // default -2
    public string WorkCountryCode { get; set; } // majburiy (ISO code - "US", "UZ")
    public string? Remarks { get; set; }         // optional
}

