using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Models;

namespace SocialMediaManager.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<SocialAccount> SocialAccounts => Set<SocialAccount>();
        public DbSet<SocialPost> SocialPosts => Set<SocialPost>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<ViewSnapshot> ViewSnapshots => Set<ViewSnapshot>();
        public DbSet<EngagementMetrics> EngagementMetricses => Set<EngagementMetrics>();

    }
}
