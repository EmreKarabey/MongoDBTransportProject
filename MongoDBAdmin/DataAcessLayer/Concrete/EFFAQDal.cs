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
    public class EFFAQDal : GenericRepository<FAQ>, IFAQDal
    {
        public EFFAQDal(IDatabaseSettings _databaseSettings) : base(_databaseSettings)
        {
        }
    }
}
