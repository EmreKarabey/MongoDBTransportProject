using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.GoogleLogin;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Login.Command.GoogleLogin
{
    public class GoogleLoginCommand : IRequest<GoogleLoginResult>
    {
        public string Token { get; set; }
    }

    public class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, GoogleLoginResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGoogleLoginService _googleLoginService;
        private readonly SignInManager<AppUser> _signInManager;



        public GoogleLoginHandler(UserManager<AppUser> userManager, IGoogleLoginService googleLoginService, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _googleLoginService = googleLoginService;
            _signInManager = signInManager;
        }

        public async Task<GoogleLoginResult> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var payload = await _googleLoginService.GoogleLoginAsync(request.Token);

            AppUser? user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new AppUser
                {
                    Email = payload.Email,
                    FullName = $"{payload.GivenName ?? "Google"} {payload.FamilyName ?? "User"}",
                    EmailConfirmed = true,
                    ImageURL = payload.Picture,
                    UserName = $"{(payload.GivenName ?? "google").ToLower()}{payload.FamilyName ?? "user"}",
                };

                var createResult = await _userManager.CreateAsync(user);

                var assigneRole = await _userManager.AddToRoleAsync(user, "Üye");

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return new GoogleLoginResult { Success = false, Message = errors };
                }
            }
            await _signInManager.SignInAsync(user, isPersistent: true);

            return new GoogleLoginResult
            {
                Success = true,
                User = user
            };

        }
    }
}

