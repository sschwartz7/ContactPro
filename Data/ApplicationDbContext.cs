using ContactPro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactPro.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Code first technique is using classes to represent the tables 
        public virtual DbSet<Contact> Contacts { get; set; } = default!;//initialized the dbset property 
        public virtual DbSet<Category> Categories { get; set; } = default!;
    }
}