﻿using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApplication1.Security
{
    public class SuperAdminHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            if (context.User.IsInRole("superadmin"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
