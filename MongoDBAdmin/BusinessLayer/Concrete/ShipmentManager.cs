using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAcessLayer.Abstract;
using EntityLayer.Entities;
using EntityLayer.Paginate;

namespace BusinessLayer.Concrete
{
    public class ShipmentManager : IShipmentService
    {
        private readonly IShipmentDal _shipmentDal;

        public ShipmentManager(IShipmentDal shipmentDal)
        {
            _shipmentDal = shipmentDal;
        }

        public async Task CreateAsync(Shipment t)
        {
            await _shipmentDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
           await _shipmentDal.DeleteAsync(Id);
        }

        public async Task<Shipment> GetByIdAsync(string Id)
        {
            return await _shipmentDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<Shipment>> GetListAsync(int size, int page)
        {
           return await _shipmentDal.GetListAsync(size, page);
        }

        public async Task<List<Shipment>> GetListAsync()
        {
            return await _shipmentDal.GetListAsync();
        }

        public async Task UpdateAsync(Shipment t, string Id)
        {
           await _shipmentDal.UpdateAsync(t, Id);
        }
    }
}
