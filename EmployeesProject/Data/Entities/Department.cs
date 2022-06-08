using System.ComponentModel.DataAnnotations;

namespace EmployeesProject.Data.Entities
{
    public class Department
    {
        public int Id { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string Name { get; set; }
        [RegularExpression(@"(\+7)?[0-9]{10}")]
        public string Phone { get; set; }
    }
}
