using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Touch.Data;
using Touch.Models;
using Touch.Services;

namespace Touch.ViewModels
{
    /// <summary>
    ///     文件夹列表ViewModel
    /// </summary>
    public class FolderListViewModel : ViewModelBase
    {
        private readonly IFolderModelService _folderModelService;
        private readonly IFolderListService _folderListService;
        /// <summary>
        ///     与data交互的model，文件夹路径list
        /// </summary>
        private FolderList _folderList;

        public FolderList FolderList
        {
            get => _folderList;
            set => Set(nameof(FolderList), ref _folderList, value);
        }
        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand<FolderModel> _openCommand;

        public  FolderListViewModel(IFolderListService folderListService,
            IFolderModelService folderModelService)
        {
            _folderListService = folderListService;
            _folderModelService = folderModelService;
            _folderList = new FolderList();
            _folderList.DatabaseHelper = DatabaseHelper.GetInstance();
            _folderList=_folderListService.GetInstanceAsync();
        }

        /// <summary>
        ///     删除点击操作
        /// </summary>
        public ICommand DeleteCommand
        {
            get { return new CommandHandler(folderModel => FolderList=_folderListService.Delete(folderModel as FolderModel,FolderList)); }
        }
        public RelayCommand<FolderModel> OpenCommand =>
            _openCommand ?? (_openCommand = new RelayCommand<FolderModel>( 
               async folderModel =>
            {
                 FolderList = await _folderListService.OpenAsync(folderModel, FolderList);
            }));
    }

}