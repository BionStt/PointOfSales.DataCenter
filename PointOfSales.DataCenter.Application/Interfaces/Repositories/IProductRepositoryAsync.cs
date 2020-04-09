﻿using PointOfSales.DataCenter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.DataCenter.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IRepositoryAsync<Product>
    {
        Task<bool> DoesBarCodeExist(string barCode);
    }
}