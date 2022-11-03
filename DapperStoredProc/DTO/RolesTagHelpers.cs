using DapperStoredProc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.DTO
{
    public class RolesTagHelpers
    {
        [HtmlTargetElement("td", Attributes = "user_role")]
        public class RolesTagHelper : TagHelper
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<Users> userManager;

            public RolesTagHelper(RoleManager<IdentityRole> roleManager, UserManager<Users> userManager)
            {
                this.roleManager = roleManager;
                this.userManager = userManager;
            }

            [HtmlAttributeName("user_role")]
            public string RoleId { get; set; }

            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                List<string> names = new List<string>();
                IdentityRole role = await roleManager.FindByIdAsync(RoleId);

                if (role != null)
                {
                    foreach (var user in userManager.Users)
                    {
                        if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                        {
                            names.Add(user.Name);
                        }
                    }
                }
                output.Content.SetContent(names.Count == 0 ? "No users" : string.Join(", ", names));

            }
        }
    }
}
