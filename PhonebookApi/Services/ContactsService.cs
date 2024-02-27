using Azure.Core;
using ErrorOr;
using PhonebookApi.Contracts.Requests;
using PhonebookApi.Contracts.Responses;
using PhonebookApi.Entities;
using PhonebookApi.Errors;
using PhonebookApi.Repositories.Interfaces;
using PhonebookApi.Services.Interfaces;

namespace PhonebookApi.Services;

public class ContactsService : IContactsService
{
    private readonly IContactsRepository contactsRepository;

    public ContactsService(IContactsRepository contactsRepository)
    {
        this.contactsRepository = contactsRepository;
    }

    public async Task<ErrorOr<ContactResponse>> CreateContactAsync(CreateContactRequest request)
    {
        // try...catch prevents issues with duplicate phone number (index).
        try
        {
            bool isConflict = await contactsRepository.PhoneNumberExistsAsync(request.PhoneNumber);
            if (isConflict) { return RepositoryErrors.Contacts.PhoneNumberConflict; }
            ContactEntity contactEntity = ContactFactory.CreateContactEntity(request);
            bool isSuccessfull = await contactsRepository.CreateContactAsync(contactEntity);
            if (!isSuccessfull) { return RepositoryErrors.Contacts.Unexpected; }
            ContactResponse contactResponse = ContactFactory.CreateContactResponse(contactEntity);
            return contactResponse;
        } catch(Exception ex)
        {
            return catchPhoneNumberConflict(ex);
        }
    }

    public async Task<ErrorOr<Deleted>> DeleteContactAsync(int id)
    {
        ContactEntity? contactEntity = await contactsRepository.GetContactByIdAsync(id, true);
        if (contactEntity == null) { return RepositoryErrors.Contacts.NotFound; }
        bool isSuccessfull = await contactsRepository.DeleteContactAsync(contactEntity);
        if (!isSuccessfull) { return RepositoryErrors.Contacts.Unexpected; }
        return Result.Deleted;
    }

    public async Task<ErrorOr<ContactResponse>> GetContactByIdAsync(int id)
    {
        ContactEntity? contactEntity = await contactsRepository.GetContactByIdAsync(id);
        if (contactEntity == null) { return RepositoryErrors.Contacts.NotFound; }
        ContactResponse contactResponse = ContactFactory.CreateContactResponse(contactEntity);
        return contactResponse;
    }

    public async Task<ErrorOr<List<ContactResponse>>> GetContactsAsync()
    {
        List<ContactEntity> contactEntities = await contactsRepository.GetContactsAsync();
        List<ContactResponse> contactsResponse = contactEntities
            .Select(ContactFactory.CreateContactResponse)
            .ToList();
        return contactsResponse;
    }

    public async Task<ErrorOr<Updated>> UpdateContactAsync(int id, UpdateContactRequest request)
    {
        // try...catch prevents issues with duplicate phone number (index).
        try
        {
            bool isConflict = await contactsRepository.PhoneNumberExistsAsync(request.PhoneNumber, id);
            if (isConflict) { return RepositoryErrors.Contacts.PhoneNumberConflict; }
            ContactEntity? contactEntity = await contactsRepository.GetContactByIdAsync(id, true);
            if (contactEntity == null) { return RepositoryErrors.Contacts.NotFound; }
            contactEntity.Update(request);
            bool isSuccessfull = await contactsRepository.UpdateContactAsync(contactEntity);
            if (!isSuccessfull) { return RepositoryErrors.Contacts.Unexpected; }
            return Result.Updated;
        }
        catch (Exception ex)
        {
            return catchPhoneNumberConflict(ex);
        }
    }

    private Error catchPhoneNumberConflict(Exception ex)
    {
        if (ex.InnerException == null) { return RepositoryErrors.Contacts.Unexpected; }
        if (ex.InnerException.Message.StartsWith("Duplicate"))
        {
            return RepositoryErrors.Contacts.PhoneNumberConflict;
        }
        return RepositoryErrors.Contacts.Unexpected;
    }
}
