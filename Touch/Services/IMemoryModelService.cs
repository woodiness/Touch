using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;
using Touch.ViewModels;

namespace Touch.Services
{
    public interface IMemoryModelService
    {
        Task<MemoryModel> GetInstanceAsync(MemoryModel memoryModel);
        MemoryModel GetNewMemoryModel(MemoryModel memoryModel, int lastKeyNo, String memoryName, List<ImageModel> imageModels);
    }
}
