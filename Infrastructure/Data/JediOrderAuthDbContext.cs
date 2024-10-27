//using JediOrderApi.Enums;
//using JediOrderApi.Models.Domain;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace JediOrderInfrastructure.Data
//{
//    public class JediOrderAuthDbContext : IdentityDbContext
//    {
//        public JediOrderAuthDbContext(DbContextOptions<JediOrderAuthDbContext> options) : base(options)
//        {           
//        }
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            var readerRoleId = "dd99353d-ae6f-4d98-9176-98a79f9ea28a";
//            var writerRoleId = "99ea951c-bba5-41be-a465-dd5a82507c3f";

//            // Seed data for 'User Roles'
//            builder.Entity<IdentityRole>().HasData(
//                new IdentityRole
//                {
//                    Id = readerRoleId,
//                    ConcurrencyStamp = readerRoleId,
//                    Name = "Reader",
//                    NormalizedName = "Reader".ToUpper(),
//                },
//                 new IdentityRole
//                 {
//                     Id = writerRoleId,
//                     ConcurrencyStamp = writerRoleId,
//                     Name = "Writer",
//                     NormalizedName = "Writer".ToUpper(),
//                 }
//            );
//        }
//    }
//}
