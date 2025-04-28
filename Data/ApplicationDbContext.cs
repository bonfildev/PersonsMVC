using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonsMVC.Models;

namespace PersonsMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonsMVC.Models.Persons> Persons { get; set; } = default!;
        public DbSet<PersonsTasks> RowItems { get; set; }

        // Use the 'new' keyword to hide the inherited 'Roles' property  
        public DbSet<PersonsRoles> Roles { get; set; }
        public DbSet<PersonsTasks> PersonsTasks { get; set; }
    }
}
