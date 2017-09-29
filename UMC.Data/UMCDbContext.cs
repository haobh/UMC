using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using UMC.Model.Models;

namespace UMC.Data
{
    public class UMCDbContext : IdentityDbContext<ApplicationUser>
    {
        public UMCDbContext() : base("UMC")
        {
            this.Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer<UMCDbContext>(null);
        }

        public DbSet<Footer> Footers { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<MenuGroup> MenuGroups { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Slide> Slides { set; get; }

        //Migration
        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }

        public static UMCDbContext Create()
        {
            return new UMCDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId });
            //builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId);

            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }
}