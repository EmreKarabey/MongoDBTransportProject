using System.Threading.Tasks;
using AutoMapper;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using MongoDBAdmin.Dto.Role;

namespace MongoDBAdmin.ViewComponents
{
    public class _UserListRolePartialComponent:ViewComponent
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public _UserListRolePartialComponent(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _roleManager.Roles.ToListAsync();

            var listDto = _mapper.Map<List<RoleListDto>>(list);
            return View(listDto);
        }
    }
}
