using System.ComponentModel.DataAnnotations;

namespace CandidateApi.Models
{
    public class Candidate
    {
        public int Id { get; set; } // Unique identifier for the database

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } // Candidate's first name

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } // Candidate's last name

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Candidate's email

        public string PhoneNumber { get; set; } // Candidate's phone number
        public string CallInterval { get; set; } // Suggested call interval
        public string LinkedInProfileUrl { get; set; } // LinkedIn profile link
        public string GitHubProfileUrl { get; set; } // GitHub profile link

        [Required(ErrorMessage = "Comment is required.")]
        public string Comment { get; set; } // Free text comment
    }
}
