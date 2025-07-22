using CDN.Directory.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDN.Directory.Infrastructure.Data
{
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<SkillsetMaster> Skillsets { get; set; }
        public DbSet<HobbyMaster> Hobbies { get; set; }
        public DbSet<MemberSkillset> MemberSkillsets { get; set; }
        public DbSet<MemberHobby> MemberHobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SkillsetMaster>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<HobbyMaster>()
                .HasIndex(h => h.Name)
                .IsUnique();

            modelBuilder.Entity<MemberSkillset>()
                .HasOne(ms => ms.Member)
                .WithMany(m => m.MemberSkillsets)
                .HasForeignKey(ms => ms.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberSkillset>()
                .HasOne(ms => ms.Skillset)
                .WithMany(s => s.MemberSkillsets)
                .HasForeignKey(ms => ms.SkillsetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberHobby>()
                .HasOne(mh => mh.Member)
                .WithMany(m => m.MemberHobbies)
                .HasForeignKey(mh => mh.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberHobby>()
                .HasOne(mh => mh.Hobby)
                .WithMany(h => h.MemberHobbies)
                .HasForeignKey(mh => mh.HobbyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
