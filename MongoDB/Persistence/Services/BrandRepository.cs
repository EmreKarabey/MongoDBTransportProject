using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using Domain.Entities;

namespace Persistence.Services
{
    public class BrandRepository : EFRepositoryAsync<Brand>,IBrandRepository
    {
        public BrandRepository(IDataBaseSettings _dataBaseSettings) : base(_dataBaseSettings)
        {
        }
    }
}
