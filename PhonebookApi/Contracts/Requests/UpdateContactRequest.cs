using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.Contracts.Requests;

public class UpdateContactRequest
{
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 255 characters long.")]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 255 characters long.")]
    public required string LastName { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [StringLength(16, MinimumLength = 4, ErrorMessage = "Phone number must be between 4 and 255 characters long.")]
    [RegularExpression(@"^\+?[0-9]{4,15}$", ErrorMessage = "Use only numbers and + sign.")]
    public required string PhoneNumber { get; set; }
}
