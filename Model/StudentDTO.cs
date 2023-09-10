using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Model
{
    public class StudentDTO
    {
        [ValidateNever]
        [StringLength(30)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        public string StudentName { get; set; }

        [EmailAddress(ErrorMessage ="Please enter valid email address")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

       // [DateCheck]
       // public DateTime AdmissionDate { get; set; }

    }
}
