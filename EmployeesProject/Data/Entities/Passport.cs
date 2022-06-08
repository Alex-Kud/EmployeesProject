using System.ComponentModel.DataAnnotations;

namespace EmployeesProject.Data.Entities
{
    public class Passport
    {
        public int Id { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string Type { get; set; }
        [RegularExpression(@"^[0-9]*$")]
        public string Number { get; set; }
    }
}
