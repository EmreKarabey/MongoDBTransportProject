using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using Domain.Entities;
using MongoDB.Driver;

namespace Persistence.Services
{
    public class FAQRepository : EFRepositoryAsync<FAQ>, IFAQRepository
    {
        public FAQRepository(IDataBaseSettings _dataBaseSettings) : base(_dataBaseSettings)
        {
        }
    }
}
