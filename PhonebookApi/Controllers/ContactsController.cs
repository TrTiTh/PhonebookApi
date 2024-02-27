using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PhonebookApi.Contracts.Requests;
using PhonebookApi.Contracts.Responses;
using PhonebookApi.Services.Interfaces;

namespace PhonebookApi.Controllers;

public class ContactsController : ApiController
{
    private readonly IContactsService contactsService;

    public ContactsController(IContactsService contactsService)
    {
        this.contactsService = contactsService;
    }

    /// <summary>
    /// Get a list of all contacts in the DB.
    /// </summary>
    /// <response code="200">Contacts fetched successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ContactResponse>), 200)]
    public async Task<IActionResult> GetContacts()
    {
        ErrorOr<List<ContactResponse>> getContactsResult = await contactsService.GetContactsAsync();
        return getContactsResult.Match(Ok, Problem);
    }

    /// <summary>
    /// Get a single contact by id.
    /// </summary>
    /// <response code="200">Contact fetched successfully.</response>
    /// <response code="404">Contact not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContactResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetContactById(int id)
    {
        ErrorOr<ContactResponse> getContactResult = await contactsService.GetContactByIdAsync(id);
        return getContactResult.Match(Ok, Problem);
    }

    /// <summary>
    /// Create a new contact that has to have a unique phone number.
    /// </summary>
    /// <response code="201">Contact created successfully.</response>
    /// <response code="400">Bad request: see errors for more details.</response>
    /// <response code="409">Phone number is already taken.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ContactResponse), 201)]
    [ProducesResponseType(typeof(IValidationError), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    public async Task<IActionResult> CreateContact(CreateContactRequest requestBody)
    {
        ErrorOr<ContactResponse> createContactResult = await contactsService.CreateContactAsync(requestBody);
        return createContactResult.Match(response =>
        {
            return CreatedAtAction(
                actionName: nameof(GetContactById),
                routeValues: new { Id = response.Id },
                value: response
            );
        }, Problem);
    }

    /// <summary>
    /// Update a contact.
    /// </summary>
    /// <response code="204">Contact updated successfully.</response>
    /// <response code="400">Bad request: see errors for more details.</response>
    /// <response code="404">Contact not found.</response>
    /// <response code="409">Phone number is already taken.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(IValidationError), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    public async Task<IActionResult> UpdateContact(int id, UpdateContactRequest requestBody)
    {
        ErrorOr<Updated> updateContactResult = await contactsService.UpdateContactAsync(id, requestBody);
        return updateContactResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Delete a contact.
    /// </summary>
    /// <response code="204">Contact deleted successfully.</response>
    /// <response code="404">Contact not found.</response>
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        ErrorOr<Deleted> deleteResult = await contactsService.DeleteContactAsync(id);
        return deleteResult.Match(_=> NoContent(), Problem);
    }
}