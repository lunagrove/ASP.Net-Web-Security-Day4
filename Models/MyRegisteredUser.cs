﻿using System.ComponentModel.DataAnnotations;

namespace Day3Paypal.Models
{
    public class MyRegisteredUser
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
