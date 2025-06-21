using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities;

public class Employee
{
    public int EmployeeID { get; set; }         
    public string FirstName { get; set; }    
    public string LastName { get; set; }      
    public string JobTitle { get; set; }      
    public int? Department { get; set; }   
    public int? Branch { get; set; }
    public string WorkCountryCode { get; set; } 
    public string? Remarks { get; set; }        
}

