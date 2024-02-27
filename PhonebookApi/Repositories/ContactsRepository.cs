using Microsoft.EntityFrameworkCore;
using PhonebookApi.Data;
using PhonebookApi.Entities;
using PhonebookApi.Repositories.Interfaces;

namespace PhonebookApi.Repositories;

public class ContactsRepository : IContactsRepository
{
    private readonly ApplicationDbContext context;

    public ContactsRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreateContactAsync(ContactEntity entity)
    {
        context.Add(entity);
        int createResult = await context.SaveChangesAsync();
        return createResult > 0;
    }

    public async Task<bool> DeleteContactAsync(ContactEntity entity)
    {
        context.Remove(entity);
        int deleteResult = await context.SaveChangesAsync();
        return deleteResult > 0;
    }

    public Task<ContactEntity?> GetContactByIdAsync(int id, bool tracking = false)
    {
        var dbSet = context.Contacts;
        var query = tracking ? dbSet : dbSet.AsNoTracking();
        return query.FirstOrDefaultAsync(contact => contact.Id == id);
    }

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, int id = -1)
    {
        var query = context.Contacts.AsNoTracking();
        return id == -1
            ? query.AnyAsync(contact => contact.PhoneNumber == phoneNumber)
            : query.AnyAsync(contact => contact.PhoneNumber == phoneNumber && contact.Id != id);
    }

    public Task<List<ContactEntity>> GetContactsAsync(bool tracking = false)
    {
        var dbSet = context.Contacts;
        var query = tracking ? dbSet : dbSet.AsNoTracking();
        return query.ToListAsync();
    }

    public async Task<bool> UpdateContactAsync(ContactEntity entity)
    {
        context.Update(entity);
        int updateResult = await context.SaveChangesAsync();
        return updateResult > 0;
    }
}
