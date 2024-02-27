using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.Contracts.Responses;

public class ContactResponse
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string PhoneNumber { get; set; }
}
