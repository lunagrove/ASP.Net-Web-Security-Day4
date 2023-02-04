using System.ComponentModel.DataAnnotations;

namespace Day3Paypal.ViewModels
{
    public class UserVM
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
