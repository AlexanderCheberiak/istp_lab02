namespace SocialNetworkApp.Models
{
    using Microsoft.EntityFrameworkCore;

    namespace SocialNetworkApp.Models
    {
        public class SocialNetworkContext : DbContext
        {
            public virtual DbSet<User> Users { get; set; }
            public virtual DbSet<Community> Communities { get; set; }
            public virtual DbSet<Post> Posts { get; set; }
            public virtual DbSet<UserCommunity> UserCommunities { get; set; }
            public virtual DbSet<Friendship> Friendships { get; set; }

            public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
                : base(options)
            {
                Database.EnsureCreated();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<UserCommunity>()
                    .HasKey(uc => new { uc.UserId, uc.CommunityId });

                modelBuilder.Entity<Friendship>()
                    .HasKey(f => new { f.UserId1, f.UserId2 });

                modelBuilder.Entity<Friendship>()
                    .HasOne(f => f.User1)
                    .WithMany(u => u.Friendships)
                    .HasForeignKey(f => f.UserId1)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Friendship>()
                    .HasOne(f => f.User2)
                    .WithMany()
                    .HasForeignKey(f => f.UserId2)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }

}
