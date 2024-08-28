using WebApplication1.Models;
using static WebApplication1.Models.Employee;

namespace WebApplication1.ViewModel
{
    public class EmployeeIndexViewModel
    {
        public Employee Employee { get; set; }
        public string PageTitle { get; set; }
    }
}
