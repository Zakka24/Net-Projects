using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfacce
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task <Stock?> GetByIdAsync(int id); // nullable perché FirstOrDefault può essere null
        Task <Stock> CreateAsync(Stock stockModel);
        Task <Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDto);
        Task <Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int id);
        Task <Stock?> GetBySymbolAsync(string symbol);
    }
}