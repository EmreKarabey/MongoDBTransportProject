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

    public class GoogleLoginResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public AppUser? User { get; set; }
    }

}
