using ErrorOr;
using PhonebookApi.Contracts.Requests;
using PhonebookApi.Contracts.Responses;

namespace PhonebookApi.Services.Interfaces;

public interface IContactsService
{
    public Task<ErrorOr<List<ContactResponse>>> GetContactsAsync();
    public Task<ErrorOr<ContactResponse>> GetContactByIdAsync(int id);
    public Task<ErrorOr<ContactResponse>> CreateContactAsync(CreateContactRequest request);
    public Task<ErrorOr<Updated>> UpdateContactAsync(int id, UpdateContactRequest request);
    public Task<ErrorOr<Deleted>> DeleteContactAsync(int id);
}
