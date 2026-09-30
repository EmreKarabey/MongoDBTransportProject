
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAcessLayer.Abstract;
using EntityLayer.Entities;
using EntityLayer.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver.Linq;

namespace BusinessLayer.Concrete
{
    public class AppUserManager : IAppUserService
    {
        private readonly IAppUserDal _appUserDal;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppUserManager(IAppUserDal appUserDal, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _appUserDal = appUserDal;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateAsync(AppUser t)
        {
            await _appUserDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            await _appUserDal.DeleteAsync(Id);
        }

        public async Task<AppUser> GetByIdAsync(string Id)
        {
            return await _appUserDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<AppUser>> GetListAsync(int size, int page)
        {
            return await _appUserDal.GetListAsync(size, page);
        }

        public async Task<List<AppUser>> GetListAsync()
        {
            return await _appUserDal.GetListAsync();
        }

        public Task<List<AppUser?>> GetUserListAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guid currentUserId = Guid.Parse(userId);

            var list = _userManager.Users.Where(n => n.Id != currentUserId).ToListAsync();

            return list ?? null;
        }

        public async Task UpdateAsync(AppUser t, string Id)
        {
            await _appUserDal.UpdateAsync(t, Id);
        }
    }
}
