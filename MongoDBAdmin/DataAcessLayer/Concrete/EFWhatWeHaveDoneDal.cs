using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Abstract;
using DataAcessLayer.Repository;
using EntityLayer.Entities;
using Microsoft.Extensions.Options;

using BusinessLayer;

namespace DataAcessLayer.Concrete
{
    public class EFWhatWeHaveDoneDal : GenericRepository<WhatWeHaveDone>, IWhatWeHaveDoneDal
    {
        public EFWhatWeHaveDoneDal(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {
        }
    }
}
