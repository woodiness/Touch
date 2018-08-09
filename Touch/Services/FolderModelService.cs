using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Touch.Data;
using Touch.Models;

namespace Touch.Services
{
    class FolderModelService : IFolderModelService
    {
        /// <summary>
        ///     获取文件夹
        /// </summary>
        /// <returns>存在返回Folder，不存在返回null</returns>
        public IAsyncOperation<StorageFolder> GetFolder(string accessToken)
        {
            try
            {
                return  StorageApplicationPermissions.FutureAccessList.GetFolderAsync(accessToken);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public  FolderList GetInstanceAsync()
        {
            var folderList = new FolderList();
            folderList.DatabaseHelper = DatabaseHelper.GetInstance();
            // 初始化list
            var query = folderList.DatabaseHelper.FolderDatabase.GetQuery();
            while (query.Read())
            {
                var folderModel = new FolderModel
                {
                    KeyNo = query.GetInt32(0),
                    FolderPath = query.GetString(1),
                    AccessToken = query.GetString(2)
                };
                // 查下这个文件夹还在不在
                var folder =  GetFolder(folderModel.AccessToken);
                if (folder != null)
                {
                    folderList.FolderModels.Add(folderModel);
                }
                else
                {
                    // 从图片数据库中删掉这个文件夹相关的图片
                    folderList.DatabaseHelper.ImageDatabase.Delete(folderModel.KeyNo);
                    // 从数据库里删掉这个文件夹
                    folderList.DatabaseHelper.FolderDatabase.Delete(folderModel.FolderPath);
                    // 从使用list里删掉这个文件夹
                    StorageApplicationPermissions.FutureAccessList.Remove(folderModel.AccessToken);
                }
            }
            return folderList;
        }
    }
}
