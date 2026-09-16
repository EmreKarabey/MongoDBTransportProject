using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using Domain.Entities;

namespace Persistence.Services
{
    public class HowItWorkRepository : EFRepositoryAsync<HowItWork>, IHowItWorkRepository
    {
        public HowItWorkRepository(IDataBaseSettings _dataBaseSettings) : base(_dataBaseSettings)
        {
        }
    }
}
