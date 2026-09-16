using System.Threading.Tasks;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using MongoDBAdmin.Dto.Role;

namespace MongoDBAdmin.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var roleList = await _roleManager.Roles.ToListAsync();
                return View(roleList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Rol listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View(new List<AppRole>());
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            if (string.IsNullOrWhiteSpace(createRoleDto.roleName))
            {
                TempData["ErrorMessage"] = "Rol adı boş bırakılamaz. Lütfen geçerli bir rol adı girin.";
                return View(createRoleDto);
            }

            try
            {
                var existingRole = await _roleManager.FindByNameAsync(createRoleDto.roleName);
                if (existingRole != null)
                {
                    TempData["ErrorMessage"] = $"'{createRoleDto.roleName}' adında bir rol zaten mevcut. Lütfen farklı bir isim seçin.";
                    return View(createRoleDto);
                }

                var role = new AppRole
                {
                    Name = createRoleDto.roleName,
                    NormalizedName = createRoleDto.roleName.ToUpper()
                };

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["ErrorMessage"] = $"Rol oluşturulamadı: {errors}";
                    return View(createRoleDto);
                }

                TempData["SuccessMessage"] = $"'{createRoleDto.roleName}' rolü başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Rol oluşturulurken beklenmedik bir hata oluştu: {ex.Message}";
                return View(createRoleDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                var entity = await _roleManager.FindByIdAsync(id.ToString());
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Silinecek rol bulunamadı. Kayıt zaten silinmiş veya geçersiz bir istek yapılmış olabilir.";
                    return RedirectToAction("Index");
                }

                var result = await _roleManager.DeleteAsync(entity);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["ErrorMessage"] = $"'{entity.Name}' rolü silinemedi: {errors}";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = $"'{entity.Name}' rolü başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Rol silinirken beklenmedik bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["ErrorMessage"] = "Geçersiz rol ID. Güncellenecek rol belirlenemedi.";
                    return RedirectToAction("Index");
                }

                var entity = await _roleManager.FindByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek rol bulunamadı. Kayıt silinmiş veya geçersiz bir istek yapılmış olabilir.";
                    return RedirectToAction("Index");
                }

                var dto = new UpdateRoleDto
                {
                    Id = entity.Id.ToString(),
                    roleName = entity.Name
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Rol bilgileri yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto updateRoleDto)
        {
            if (string.IsNullOrWhiteSpace(updateRoleDto.roleName))
            {
                TempData["ErrorMessage"] = "Rol adı boş bırakılamaz. Lütfen geçerli bir rol adı girin.";
                return View(updateRoleDto);
            }

            try
            {
                var entity = await _roleManager.FindByIdAsync(updateRoleDto.Id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek rol bulunamadı. Kayıt silinmiş veya geçersiz bir istek yapılmış olabilir.";
                    return RedirectToAction("Index");
                }

                var existingRole = await _roleManager.FindByNameAsync(updateRoleDto.roleName);
                if (existingRole != null && existingRole.Id != entity.Id)
                {
                    TempData["ErrorMessage"] = $"'{updateRoleDto.roleName}' adında başka bir rol zaten mevcut. Lütfen farklı bir isim seçin.";
                    return View(updateRoleDto);
                }

                var oldName = entity.Name;
                entity.Name = updateRoleDto.roleName;
                entity.NormalizedName = updateRoleDto.roleName.ToUpper();

                var result = await _roleManager.UpdateAsync(entity);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["ErrorMessage"] = $"'{oldName}' rolü güncellenemedi: {errors}";
                    return View(updateRoleDto);
                }

                TempData["SuccessMessage"] = $"'{oldName}' rolü '{updateRoleDto.roleName}' olarak başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Rol güncellenirken beklenmedik bir hata oluştu: {ex.Message}";
                return View(updateRoleDto);
            }
        }
    }
}
