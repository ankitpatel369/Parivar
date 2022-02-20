using Microsoft.AspNetCore.Identity;
using Parivar.Data.DbContext;
using Parivar.Dto.Enum;
using Parivar.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Parivar
{
    public static class ParivarIdentityDataInitializer
    {
        public static void SeedData(UserManager<FamilyUser> userManager, RoleManager<Role> roleManager, IRelationShipMasterService relationShip)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedRelationShips(relationShip);
        }

        private static void SeedUsers(UserManager<FamilyUser> userManager)
        {
            var user = new FamilyUser
            {
                FirstName = "Ghanshyambhai",
                LastName = "Chodvadiya",
                FatherName = "Bhikhabhai",
                UserName = "cgankit@gmail.com",
                ProfilePic = "",
                PhoneNumber = "+91 999-999-9999",
                NormalizedUserName = "Admin",
                Email = "cgankit@gmail.com",
                NormalizedEmail = "cgankit@gmail.com",
                EmailConfirmed = true,
                IsActive = true,
                NoOfMembers = 5
            };
            if (userManager.FindByEmailAsync(user.UserName).Result == null)
            {
                var result = userManager.CreateAsync(user, "P@ssword123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, UserRoles.SystemAdmin).Wait();
                }
            }


            var developerUser = new FamilyUser
            {
                FirstName = "Ankit",
                LastName = "Chodvadiya",
                FatherName = "Ghanshyambhai",
                UserName = "cgpatel@live.com",
                ProfilePic = "",
                PhoneNumber = "+91 999-999-9999",
                NormalizedUserName = "Anc Patel",
                Email = "cgpatel@live.com",
                NormalizedEmail = "cgpatel@live.com",
                EmailConfirmed = true,
                IsActive = true,
                NoOfMembers = 0
            };
            if (userManager.FindByEmailAsync(developerUser.UserName).Result != null) return;
            var developeResult = userManager.CreateAsync(developerUser, "P@ssword123").Result;

            if (developeResult.Succeeded)
            {
                userManager.AddToRoleAsync(developerUser, UserRoles.Developer).Wait();
            }
        }

        private static void SeedRoles(RoleManager<Role> roleManager)
        {
            #region User Roles
            Dictionary<string, string> normalizedName = new Dictionary<string, string>
            {
                { "SystemAdmin", "System Admin"},
                { "FamilyMember", "Family Member"},
                { "Developer", "Developer"}
            };

            var existrolesList = roleManager.Roles.Select(x => x.Name).ToList();
            if (existrolesList.Any())
            {
                var notExirst = normalizedName.Keys.Except(existrolesList);
                foreach (var notRole in notExirst)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x.Key == notRole).Value;
                    var roleResult = roleManager.CreateAsync(new Role { Name = notRole, NormalizedName = normalized, DisplayRoleName = normalized }).Result;
                }
            }
            else
            {
                foreach (var objRole in normalizedName.Keys)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x.Key == objRole).Value;
                    IdentityResult roleResult = roleManager.CreateAsync(new Role { Name = objRole, NormalizedName = normalized, DisplayRoleName = normalized }).Result;
                }
            }
            #endregion
        }

        private static void SeedRelationShips(IRelationShipMasterService relationShip)
        {
            #region RelationShip
            List<string> normalizedName = new List<string> { "Son", "Daughter", "Daughter in law", "Dad", "Mother", "Uncle", "Aunt" };

            var existrelationShipList = relationShip.GetAll().Select(x => x.Relation).ToList();
            if (existrelationShipList.Any())
            {
                var notExirst = normalizedName.Except(existrelationShipList);
                foreach (var notRole in notExirst)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x == notRole);
                    relationShip.Add(new Data.DbModel.RelationShipMaster { Relation = normalized, IsActive = true });
                    relationShip.Save();
                }
            }
            else
            {
                foreach (var objRole in normalizedName)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x == objRole);
                    relationShip.Add(new Data.DbModel.RelationShipMaster { Relation = normalized, IsActive = true });
                    relationShip.Save();
                }
            }
            #endregion
        }
    }
}
