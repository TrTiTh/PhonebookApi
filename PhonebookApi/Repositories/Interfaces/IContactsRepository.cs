using PhonebookApi.Entities;

namespace PhonebookApi.Repositories.Interfaces;

public interface IContactsRepository
{
    public Task<List<ContactEntity>> GetContactsAsync(bool tracking = false);
    public Task<ContactEntity?> GetContactByIdAsync(int id, bool tracking = false);
    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, int id = -1);
    public Task<bool> CreateContactAsync(ContactEntity entity);
    public Task<bool> UpdateContactAsync(ContactEntity entity);
    public Task<bool> DeleteContactAsync(ContactEntity entity);
}
