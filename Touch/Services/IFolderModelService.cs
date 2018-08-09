using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Touch.Models;

namespace Touch.Services
{
    public interface IFolderModelService
    {
        /// <summary>
        ///     获取文件夹
        /// </summary>
        /// <returns>存在返回Folder，不存在返回null</returns>
        IAsyncOperation<StorageFolder> GetFolder(string accessToken);
        /// <summary>
        /// 异步获取实例
        /// </summary>
        /// <returns></returns>
        FolderList GetInstanceAsync();
    }
}
