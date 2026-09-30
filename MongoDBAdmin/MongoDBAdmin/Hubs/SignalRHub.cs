using System.Net.NetworkInformation;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Linq;

namespace MongoDBAdmin.Hubs
{
    [Authorize(Roles = "Admin,Moderatör")]
    public class SignalRHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBrandService _brandService;
        private readonly IOfferService _offerService;
        private readonly ISliderService _sliderService;
        private readonly IWhatWeHaveDoneService _whatWeHaveDoneService;
        private readonly IFAQService _faqService;
        private readonly IShipmentService _shipmentService;

        public SignalRHub(UserManager<AppUser> userManager, IBrandService brandService, IOfferService offerService, ISliderService sliderService, IWhatWeHaveDoneService whatWeHaveDoneService, IFAQService faqService, IShipmentService shipmentService)
        {
            _userManager = userManager;
            _brandService = brandService;
            _offerService = offerService;
            _sliderService = sliderService;
            _whatWeHaveDoneService = whatWeHaveDoneService;
            _faqService = faqService;
            _shipmentService = shipmentService;
        }

        public async Task SendStatics()
        {
            var userCount = _userManager.Users.Count();
            await Clients.All.SendAsync("UsersCount",userCount);

            var activeUsersCount = _userManager.Users.Where(n => n.LockoutEnd < DateTime.UtcNow || n.LockoutEnd == null).Count();
            await Clients.All.SendAsync("ActiveUsersCount",activeUsersCount);

            var activeUsersEmailCount = _userManager.Users.Where(n => n.EmailConfirmed).Count();
            await Clients.All.SendAsync("ActiveUsersEmailCount", activeUsersEmailCount);

            var bannedUsersCount = _userManager.Users.Where(n => n.LockoutEnd > DateTime.UtcNow).Count();
            await Clients.All.SendAsync("BannedUsersCount", bannedUsersCount);
        }

        public async Task SendBrandStatics()
        {
            var brandsList = await _brandService.GetListAsync();
            var brands = brandsList;
            
            var totalBrands = brands.Count;
            var activeBrands = brands.Count(x => x.IsStatus);
            var passiveBrands = totalBrands - activeBrands;

            await Clients.All.SendAsync("TotalBrandsCount", totalBrands);
            await Clients.All.SendAsync("ActiveBrandsCount", activeBrands);
            await Clients.All.SendAsync("PassiveBrandsCount", passiveBrands);
        }

        public async Task SendOfferStatics()
        {
            var offersList = await _offerService.GetListAsync();
            var offers = offersList;

            var totalOffers = offers.Count;
            var activeOffers = offers.Count(x => x.IsStatus);
            var passiveOffers = totalOffers - activeOffers;

            await Clients.All.SendAsync("TotalOffersCount", totalOffers);
            await Clients.All.SendAsync("ActiveOffersCount", activeOffers);
            await Clients.All.SendAsync("PassiveOffersCount", passiveOffers);
        }

        public async Task SendSliderStatics()
        {
            var slidersList = await _sliderService.GetListAsync(1000, 1);
            var totalSliders = slidersList.Items.Count;

            await Clients.All.SendAsync("TotalSlidersCount", totalSliders);
        }

        public async Task SendWhatWeHaveDoneStatics()
        {
            var dataList = await _whatWeHaveDoneService.GetListAsync();
            var items = dataList;

            var totalCount = items.Count;
            var activeCount = items.Count(x => x.IsActive);
            var passiveCount = totalCount - activeCount;

            await Clients.All.SendAsync("TotalWhatWeHaveDoneCount", totalCount);
            await Clients.All.SendAsync("ActiveWhatWeHaveDoneCount", activeCount);
            await Clients.All.SendAsync("PassiveWhatWeHaveDoneCount", passiveCount);
        }

        public async Task SendFAQStatics()
        {
            var dataList = await _faqService.GetListAsync();
            var items = dataList;

            var totalCount = items.Count;
            var activeCount = items.Count(x => x.IsActive);
            var passiveCount = totalCount - activeCount;

            await Clients.All.SendAsync("TotalFAQCount", totalCount);
            await Clients.All.SendAsync("ActiveFAQCount", activeCount);
            await Clients.All.SendAsync("PassiveFAQCount", passiveCount);
        }

        public async Task SendShipmentStatics()
        {
            var dataList = await _shipmentService.GetListAsync();

            var items = dataList;

            var passiveShipment = items.Where(n => !n.IsActive).Count();
            var activeShipment = items.Where(n => n.IsActive).Count();
            var isBeingPreparedShipment = items.Count(n =>n.Trackings != null &&n.Trackings.Any() &&n.Trackings.LastOrDefault().TrackingStatus == "Sipariþiniz hazýrlanýyor");
            var shipped = items.Count(n =>n.Trackings != null &&n.Trackings.Any() &&n.Trackings.LastOrDefault().TrackingStatus == "Kargoya verildi");
            var outForDelivery = items.Count(n =>n.Trackings != null &&n.Trackings.Any() &&n.Trackings.LastOrDefault().TrackingStatus == "Kargo daðýtýma çýkartýldý");
            var delivered = items.Count(n =>n.Trackings != null &&n.Trackings.Any() &&n.Trackings.LastOrDefault().TrackingStatus == "Teslim edildi");

            await Clients.All.SendAsync("PassiveShipment",passiveShipment);
            await Clients.All.SendAsync("ActiveShipment", activeShipment);
            await Clients.All.SendAsync("IsBeingPreparedShipment", isBeingPreparedShipment);
            await Clients.All.SendAsync("Shipped", shipped);
            await Clients.All.SendAsync("OutForDelivery", outForDelivery);
            await Clients.All.SendAsync("Delivered", delivered);

        }
    }
}
