using System.Linq;
using Application.Services.Repository;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace MongoDBProject.Hubs
{
    public class SignalRHub:Hub
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly UserManager<AppUser> _userManager;

        public SignalRHub(IShipmentRepository shipmentRepository, UserManager<AppUser> userManager)
        {
            _shipmentRepository = shipmentRepository;
            _userManager = userManager;
        }

        public async Task SendStatics()
        {
            var deliveredCount = await _shipmentRepository.OnlyListAsync(predicate:N=>N.CurrentStatus== "Teslim edildi");
            await Clients.All.SendAsync("DeliveredCount", deliveredCount.Count);

            var list = await _shipmentRepository.OnlyListAsync();
            await Clients.All.SendAsync("DestinationCityCount", list.DistinctBy(n=>n.DestinationCity).Count());

            var usersInRole = await _userManager.GetUsersInRoleAsync("Üye");
            await Clients.All.SendAsync("UserCount",usersInRole.Count);

            var preparingCount = await _shipmentRepository.OnlyListAsync(predicate: N => N.CurrentStatus == "Siparişiniz hazırlanıyor");
            await Clients.All.SendAsync("PreparingCount", preparingCount.Count);

        }
    }
}
