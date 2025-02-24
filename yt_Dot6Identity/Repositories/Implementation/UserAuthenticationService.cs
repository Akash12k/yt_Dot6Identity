﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using yt_Dot6Identity.Models.Domain;
using yt_Dot6Identity.Models.DTO;
using yt_Dot6Identity.Repositories.Abstract;

namespace yt_Dot6Identity.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenticationService(SignInManager<ApplicationUser> signManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signManager = signManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Status> LoginAsyc(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid UserName";
                return status;
            }
            //We will match our password
            if (!await userManager.CheckPasswordAsync(user, model.password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }
            var signInResult = await signManager.PasswordSignInAsync(user, model.password, false, true);
            if (!signInResult.Succeeded) { 
            var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName)
                };
                foreach (var userRole in userRoles) {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }
                status.StatusCode = 1;
                status.Message = "Logged in Successfully";
                return status;
            }
            else if(signInResult.IsLockedOut){
                status.StatusCode = 0;
                status.Message = "User Locked out";
                return status;
            }
            else {
                status.StatusCode = 0;
                status.Message = "Error in loggin in";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
           await signManager.SignOutAsync();
        }

        public async Task<Status> RegsitrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if(userExists!=null)
            {
                status.StatusCode =0;
                status.Message = "User Already exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Email=model.Email,
                    UserName=model.Username,
                    EmailConfirmed=true,
            };
            var result=await userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                status.StatusCode=0;
                status.Message = "User Creation Failed";
                return status;
            }

            // role management

            if(!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if(!await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user,model.Role);
            }
            status.StatusCode = 1;
            status.Message = "User has Registered Successfully";
            return status;
        }
    }
}
