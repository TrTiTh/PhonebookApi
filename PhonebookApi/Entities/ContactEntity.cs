
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhonebookApi.Contracts.Requests;
using PhonebookApi.Contracts.Responses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace PhonebookApi.Entities;

[Index(nameof(PhoneNumber), IsUnique = true)]
public class ContactEntity
{
    public int Id { get; set; }

    [Column(TypeName = "VARCHAR(255)")]
    public required string FirstName { get; set; }

    [Column(TypeName = "VARCHAR(255)")]
    public required string LastName { get; set; }

    [Column(TypeName = "VARCHAR(16)")]
    public required string PhoneNumber { get; set; }

    public void Update(UpdateContactRequest request)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        FirstName = request.FirstName.ToUpper();
        LastName = request.LastName.ToUpper();
        PhoneNumber = request.PhoneNumber;
    }
}

public static class ContactFactory
{
    public static ContactEntity CreateContactEntity(CreateContactRequest request)
    {
        return new ContactEntity
        {
            FirstName = request.FirstName.ToUpper(),
            LastName = request.LastName.ToUpper(),
            PhoneNumber = request.PhoneNumber
        };
    }

    public static ContactResponse CreateContactResponse(ContactEntity entity)
    {
        return new ContactResponse
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PhoneNumber = entity.PhoneNumber,
        };
    }
}
