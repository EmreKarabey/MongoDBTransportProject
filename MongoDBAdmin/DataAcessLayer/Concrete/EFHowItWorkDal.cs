using BusinessLayer;
using DataAcessLayer.Abstract;
using DataAcessLayer.Repository;
using EntityLayer.Entities;

namespace DataAcessLayer.Concrete
{
    public class EFHowItWorkDal : GenericRepository<HowItWork>, IHowItWorkDal
    {
        public EFHowItWorkDal(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {
        }
    }
}
