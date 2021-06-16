using blazor_inlineEditor.CustomAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace blazor_inlineEditor.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }

        [LessThanCurrentDate(ErrorMessage = "Please pick date it the past")]
        public DateTime Birthday { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Please pick valid value")]
        public Gender Gender { get; set; }

        [Range(0, 300, ErrorMessage = "Please enter size between 0 and 300cm")]
        public int HeightInCentimeters { get; set; }

        [Range(0, 3, ErrorMessage = "Please enter size between 0 and 3m")]
        public double HeightInMeters { get; set; }

        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Please enter 6+ characters")]
        public string Password { get; set; }
    }

    public enum Gender
    {
        [Display(Name = "Female")]
        FEMALE,
        [Display(Name = "Male")]
        MALE,
        [Display(Name = "Diverse")]
        DIVERSE
    }
}
