using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Touch.Data;
using Touch.Models;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;

namespace Touch.Services
{
    class FolderListService : IFolderListService
    {

        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IFolderModelService _folderModelService;


        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="folderModelService">身份服务。</param>
        public FolderListService(IFolderModelService folderModelService)
        {
            _folderModelService = folderModelService;
        }

        /// <summary>
        /// 生成文件夹列表，并在列表尾加入“添加文件夹选项”
        /// 需要注意的是，之后岁FolderList操作时，需要先删去“添加文件夹选项”，操完结束后，在添回“添加文件夹选项”。
        /// 可以尝试直接把“添加文件夹选项”嵌到View里，但本版本由于时间原因，没有尝试
        /// </summary>
        /// <returns></returns>
        public FolderList GetInstanceAsync()
        {
            var folderList = new FolderList();
            folderList.DatabaseHelper = DatabaseHelper.GetInstance();
            folderList = _folderModelService.GetInstanceAsync();
            // 最后一个添加文件夹选项
            folderList.FolderModels.Add(new FolderModel
            {
                FolderPath = new ResourceLoader().GetString("AddFolder"),
                ItemSymbol = Application.Current.Resources["Add"] as string,
                IsDeleteVisibility = Visibility.Collapsed
            });
            return folderList;

        }
        /// <summary>
        /// 向文件夹列表中添加一个文件夹路径
        /// </summary>
        /// <param name="folderModel"></param>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public FolderList Add(FolderModel folderModel, FolderList folderList)
        {
            if (folderList.FolderModels.Contains(folderModel))
                return folderList;
            folderList.DatabaseHelper.FolderDatabase.Insert(folderModel.FolderPath, folderModel.AccessToken);
            FolderModel addFolderModel = new FolderModel
            {
                FolderPath = new ResourceLoader().GetString("AddFolder"),
                ItemSymbol = Application.Current.Resources["Add"] as string,
                IsDeleteVisibility = Visibility.Collapsed
            };
            folderList.FolderModels.Remove(addFolderModel);
            folderList.FolderModels.Add(folderModel);
            folderList.FolderModels.Add(addFolderModel);
            return folderList;
        }
        /// <summary>
        /// 从文件夹列表中，删除一个文件夹路径
        /// </summary>
        /// <param name="folderModel"></param>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public FolderList Delete(FolderModel folderModel,  FolderList folderList)
        {
            if (!folderList.FolderModels.Contains(folderModel))
                return folderList;
            // 从图片数据库中删掉这个文件夹相关的图片
            folderList.DatabaseHelper.ImageDatabase.Delete(folderModel.KeyNo);
            // 从数据库里删掉这个文件夹
            folderList.DatabaseHelper.FolderDatabase.Delete(folderModel.FolderPath);
            // 从使用list里删掉这个文件夹
            StorageApplicationPermissions.FutureAccessList.Remove(folderModel.AccessToken);
            folderList.FolderModels.Remove(folderModel);
            return folderList;
        }
        /// <summary>
        /// 添加新文件夹时的具体添加操作
        /// 在这里应该可以实现递归添加
        /// </summary>
        /// <param name="folderModel"></param>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public async Task<FolderList> OpenAsync(FolderModel folderModel, FolderList folderList)
        {
            if (folderModel == null)
                return folderList;
            if (folderModel.IsDeleteVisibility == Visibility.Collapsed)
            {
                // 如果是添加新文件夹的按钮
                var folderPicker = new FolderPicker();
                folderPicker.FileTypeFilter.Add("*");
                folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                var folder = await folderPicker.PickSingleFolderAsync();
                if (folder == null)
                    return folderList;
                var accessToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
                folderModel = new FolderModel
                {
                    FolderPath = folder.Path,
                    AccessToken = accessToken
                };
                folderList = Add(folderModel, folderList);
            }
            else
            {
                var folder = await _folderModelService.GetFolder(folderModel.AccessToken);
                await Launcher.LaunchFolderAsync(folder);
            }
            return folderList;
        }
        /// <summary>
        /// 从FolderList中移除尾部“添加文件夹选项”一项
        /// </summary>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public FolderList RemoveAdd(FolderList folderList)
        {
            FolderModel addFolderModel = new FolderModel
            {
                FolderPath = new ResourceLoader().GetString("AddFolder"),
                ItemSymbol = Application.Current.Resources["Add"] as string,
                IsDeleteVisibility = Visibility.Collapsed
            };
            folderList.FolderModels.Remove(addFolderModel);
            return folderList;
        }
        /// <summary>
        /// 向FolderList尾部添加“添加文件夹选项”一项
        /// </summary>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public FolderList BackAdd(FolderList folderList)
        {
            FolderModel addFolderModel = new FolderModel
            {
                FolderPath = new ResourceLoader().GetString("AddFolder"),
                ItemSymbol = Application.Current.Resources["Add"] as string,
                IsDeleteVisibility = Visibility.Collapsed
            };
            folderList.FolderModels.Add(addFolderModel);
            return folderList;
        }
    }
}
