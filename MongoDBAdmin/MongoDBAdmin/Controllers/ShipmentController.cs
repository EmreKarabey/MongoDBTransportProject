using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.Shipment;

namespace MongoDBAdmin.Controllers
{
    [Authorize(Roles = "Admin,Moderatör")]
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ShipmentController(IShipmentService shipmentService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _shipmentService = shipmentService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _shipmentService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kargo listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(CreateShipmentDto createShipmentDto)
        {
            Random rnd = new Random();
            var createShipmentTrackingDto = new CreateShipmentTrackingDto
            {
                Description = "Siparişiniz hazırlanıyor.",
                Location = createShipmentDto.OriginCity,
                TrackingStatus= "Siparişiniz hazırlanıyor",
                EventDate=DateTime.UtcNow
            };

            createShipmentDto.SenderName = "MongoDB";
            var user = await _userManager.FindByIdAsync((createShipmentDto.ReceiverUserId).ToString());
            createShipmentDto.ReceiverName = user.FullName;
            createShipmentDto.CreatedDate = DateTime.UtcNow;
            createShipmentDto.CurrentStatus = "Siparişiniz hazırlanıyor";
            
            createShipmentDto.Trackings.Add(createShipmentTrackingDto);
            createShipmentDto.TrackingNumber = rnd.Next(1000000, 9999999).ToString();

            var entity = _mapper.Map<Shipment>(createShipmentDto);

            await _shipmentService.CreateAsync(entity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddTracking(string shipmentId, string location, string trackingStatus)
        {
            var shipment = await _shipmentService.GetByIdAsync(shipmentId);
            if (shipment != null)
            {

                if (string.IsNullOrWhiteSpace(location) && shipment.Trackings != null && shipment.Trackings.Any())
                {
                    location = shipment.Trackings.Last().Location;
                }

                if (string.IsNullOrWhiteSpace(trackingStatus) && shipment.Trackings != null && shipment.Trackings.Any())
                {
                    trackingStatus = shipment.Trackings.Last().Description;
                }

                var newTracking = new ShipmentTracking
                {
                    Description = trackingStatus, 
                    Location = location,
                    TrackingStatus = trackingStatus,
                    EventDate = DateTime.UtcNow
                };

                shipment.CurrentStatus = trackingStatus;

                shipment.Trackings ??= new List<ShipmentTracking>();
                shipment.Trackings.Add(newTracking);
                shipment.CurrentStatus = trackingStatus;

                if (trackingStatus == "Teslim edildi")
                {
                    shipment.IsActive = false;
                }
                else
                {
                    shipment.IsActive = true;
                }

                await _shipmentService.UpdateAsync(shipment, shipmentId);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _shipmentService.DeleteAsync(id);
                TempData["Success"] = "Sipariş kalıcı olarak silindi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Silme işlemi başarısız: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

 
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment == null)
            {
                TempData["Error"] = "Sipariş bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(shipment);
        }

   
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment == null)
            {
                TempData["Error"] = "Sipariş bulunamadı.";
                return RedirectToAction("Index");
            }

            var dto = _mapper.Map<UpdateShipmentDto>(shipment);
            return View(dto);
        }

       
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(UpdateShipmentDto updateShipmentDto)
        {
            try
            {
                var shipment = _mapper.Map<Shipment>(updateShipmentDto);
                
                if (shipment.Trackings != null && shipment.Trackings.Any())
                {
                    shipment.CurrentStatus = shipment.Trackings.Last().TrackingStatus;
                    shipment.IsActive = shipment.Trackings.Last().TrackingStatus != "Teslim edildi";
                }

                await _shipmentService.UpdateAsync(shipment, updateShipmentDto.ShipmentId);
                TempData["Success"] = "Sipariş başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Güncelleme başarısız: {ex.Message}";
                return View(updateShipmentDto);
            }
        }

        // ===== DELETE TRACKING (AJAX) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTracking([FromBody] DeleteTrackingRequest request)
        {
            try
            {
                var shipment = await _shipmentService.GetByIdAsync(request.ShipmentId);
                if (shipment == null)
                    return Json(new { success = false, message = "Sipariş bulunamadı." });

                if (shipment.Trackings == null || request.TrackingIndex < 0 || request.TrackingIndex >= shipment.Trackings.Count)
                    return Json(new { success = false, message = "Geçersiz takip kaydı." });

                shipment.Trackings.RemoveAt(request.TrackingIndex);

                // Son tracking'e göre durumu güncelle
                if (shipment.Trackings.Any())
                {
                    shipment.CurrentStatus = shipment.Trackings.Last().TrackingStatus;
                    shipment.IsActive = shipment.Trackings.Last().TrackingStatus != "Teslim edildi";
                }

                await _shipmentService.UpdateAsync(shipment, request.ShipmentId);
                return Json(new { success = true, message = "Takip kaydı silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }
    }

    public class DeleteTrackingRequest
    {
        public string ShipmentId { get; set; }
        public int TrackingIndex { get; set; }
    }
}
