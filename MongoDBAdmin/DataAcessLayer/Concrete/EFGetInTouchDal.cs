using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataAcessLayer.Abstract;
using DataAcessLayer.Repository;
using EntityLayer.Entities;

namespace DataAcessLayer.Concrete
{
    public class EFGetInTouchDal : GenericRepository<GetInTouch>, IGetInTouchDal
    {
        public EFGetInTouchDal(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {
        }
    }
}
