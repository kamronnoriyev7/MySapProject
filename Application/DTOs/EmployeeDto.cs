namespace MySapProject.Application.DTOs;

public class EmployeeDto
{
    public int? EmployeeId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string JobTitle { get; set; }
    public int? Department { get; set; } = -2;
    public int? Branch { get; set; } = -2;
    public string WorkCountryCode { get; set; }
    public string Remarks { get; set; }
}

