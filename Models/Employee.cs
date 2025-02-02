namespace Models;

public class Employee:IEmployee
{
    public virtual Guid EmployeeId { get; set; }
    public WorkRole Role { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    //Navigation properties
    public virtual List<IZoo> Zoos { get; set; }    
}