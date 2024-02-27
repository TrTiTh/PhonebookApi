using Microsoft.EntityFrameworkCore;
using PhonebookApi.Entities;

namespace PhonebookApi.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {}

    public DbSet<ContactEntity> Contacts { get; set; }
}
