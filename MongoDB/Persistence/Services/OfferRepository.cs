using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using Domain.Entities;

namespace Persistence.Services
{
    public class OfferRepository : EFRepositoryAsync<Offer>,IOfferRepository
    {
        public OfferRepository(IDataBaseSettings _dataBaseSettings) : base(_dataBaseSettings)
        {
        }
    }
}
