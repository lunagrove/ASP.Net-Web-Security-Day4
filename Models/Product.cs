using System.ComponentModel.DataAnnotations;

namespace Day3Paypal.Models
{
    // Instant Payment Notification
    public class Product
    {
        [Display(Name = "ID")]
        [Key] // Define primary key.
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string Image { get; set; }
 
    }
}
