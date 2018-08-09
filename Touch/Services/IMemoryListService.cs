using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;

namespace Touch.Services
{
    public interface IMemoryListService
    {
        Task<MemoryList> GetInstanceAsync();
        MemoryList Add(MemoryModel memoryModel, MemoryList memoryList);
        MemoryList Delete(MemoryModel memoryModel, MemoryList memoryList);
    }
}
