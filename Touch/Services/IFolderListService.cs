using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Data;
using Touch.Models;
using Touch.ViewModels;

namespace Touch.Services
{
    public interface IFolderListService
    {
        FolderList GetInstanceAsync();
        FolderList Add(FolderModel folderModel,  FolderList folderList);
        FolderList Delete(FolderModel folderModel,  FolderList folderList);
        Task<FolderList> OpenAsync(FolderModel folderModel, FolderList folderList);
        FolderList RemoveAdd(FolderList folderList);
        FolderList BackAdd(FolderList folderList);
    }
}

