using Backend_AYNI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend_AYNI
{
    public partial class DatabaseContext : IdentityDbContext<UserModel>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }

        public virtual DbSet<ForumModel> Forums { get; set; } = null!;
        public virtual DbSet<UserModel> Users { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //Generación uuid tablas modelo
            builder.Entity<ForumModel>(o =>
                o.Property(x => x.Id)
                .HasDefaultValue("uuid_generate_v4()")
                .ValueGeneratedOnAdd());

            base.OnModelCreating(builder);
        }
    }
}
