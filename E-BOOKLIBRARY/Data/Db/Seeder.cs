using E_BOOKLIBRARY.Data.Static;
using E_BOOKLIBRARY.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Db
{
    public class Seeder
    {
        //private readonly BookLibraryContext _ctx;
        //private readonly UserManager<User> _userMgr;
        //private readonly RoleManager<IdentityRole> _roleMgr;

        //public Seeder(BookLibraryContext ctx, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _ctx = ctx;
        //    _userMgr = userManager;
        //    _roleMgr = roleManager;
        //}

        public static void SeedMe(IApplicationBuilder applicationbuilder)
        {
            using (var servicescope = applicationbuilder.ApplicationServices.CreateScope())
            {
                var context = servicescope.ServiceProvider.GetService<BookLibraryContext>();

                context.Database.EnsureCreated();

                //Category
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>()
                    {
                        new Category()
                        {
                            Id = "2efdb4e9-0fb3-46a9-94c2-1c90ec9163f1",
                            Name = "Introductory programming"
                        },
                        new Category()
                        {
                            Id = "0e03c9b3-c80d-496c-a8d3-2f0e26a84e7f",
                            Name = "Object-Oriented Design"
                        },
                        new Category()
                        {
                            Id = "392f8d56-a313-4531-a098-519e21a824bf",
                            Name = "C# Programming"
                        },
                        new Category()
                        {
                            Id = "622a1csa-90ad-6302-a165-661ey17824dd",
                            Name = "Algorithms and Data Structures"
                        },
                        new Category()
                        {
                            Id = "392a1c77-b90q-6302-a098-419e21a824bf",
                            Name = "Game Programming"
                        },
                    });
                    context.SaveChanges();
                }

                //Book
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Id = "3qdba4e9-0fb3-46a9-7f34-1c20ec2363g9",
                            Title = "Python Crash Course",
                            ISBN = "1491974563",
                            Description = "This is an introductory book to Python programming",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CategoryId = "2efdb4e9-0fb3-46a9-94c2-1c90ec9163f1",
                            Image = "https://randomuser.me/portraits/men93.jpg"
                        },
                        new Book()
                        {
                            Id = "2efdb4e9-0fb3-a7d8-94c2-1c90ec9122a7",
                            Title = "Head First C#",
                            ISBN = "1321974405",
                            Description = "C# book for fundamentals of C# and game development with Unity",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CategoryId = "0e03c9b3-c80d-496c-a8d3-2f0e26a84e7f",
                            Image = "https://randomuser.me/portraits/men94.jpg"
                        },
                        new Book()
                        {
                            Id = "2efdb4e9-2ea8-46a9-94c2-1c90db7863e6",
                            Title = "Introduction to Algorithms",
                            ISBN = "1491976719",
                            Description = "Introduction to the fundamentals of Algorithms",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CategoryId = "622a1csa-90ad-6302-a165-661ey17824dd",
                            Image = "https://randomuser.me/portraits/men95.jpg"
                        },
                        new Book()
                        {
                            Id = "2efdb4e9-0fb3-34e1-94c2-1c22ec9163f1",
                            Title = "Unity from Zero to Proficiency (Beginner)",
                            ISBN = "1435627798",
                            Description = "Introduction to 3D game development in Unity Engine",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CategoryId = "392a1c77-b90q-6302-a098-419e21a824bf",
                            Image = "https://randomuser.me/portraits/men96.jpg"
                        },
                        new Book()
                        {
                            Id = "2efdb4e9-0fb3-46a9-94c2-4f54bb9163a2",
                            Title = "Head First C#",
                            ISBN = "1381676755",
                            Description = "Learning C# by Developing Games with Unity 2020",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CategoryId = "392f8d56-a313-4531-a098-519e21a824bf",
                            Image = "https://randomuser.me/portraits/men97.jpg"
                        },
                    });
                    context.SaveChanges();
                }

                //UserBooks
                if (!context.UserBooks.Any())
                {

                    context.UserBooks.AddRange(new List<UserBook>()

                        {
                          new UserBook()
                          {
                              UserId = "369a7bb8-6eaa-459e-8feb-c9f608c20049",
                              BookId = "2efdb4e9-0fb3-34e1-94c2-1c22ec9163f1"
                          },

                          new UserBook()
                          {
                              UserId = "5452a596-fb44-4c90-b68f-2b24898cd15f",
                              BookId= "2efdb4e9-0fb3-46a9-94c2-4f54bb9163a2"
                          }
                        }
                    );
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                string adminUserEmail = "admin@myebooklib.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

                if (adminUser == null)
                {
                    var newAdminUser = new AppUser
                    {
                        FirstName = "Taofeek",
                        LastName = "Abefe",
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Id = "369a7bb8-6eaa-459e-8feb-c9f608c20049",
                    };

                    await userManager.CreateAsync(newAdminUser, "Coding@123?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                //
                string appUserEmail = "user@myebooklib.com";
                var appUser = await userManager.FindByEmailAsync(appUserEmail);

                if (appUser == null)
                {
                    var newAppUser = new AppUser
                    {
                        FirstName = "Kamil",
                        LastName = "Adewale",
                        UserName = appUserEmail,
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Id = "5452a596-fb44-4c90-b68f-2b24898cd15f",

                    };

                    await userManager.CreateAsync(newAppUser, "Coding@123?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
