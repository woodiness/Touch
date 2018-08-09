using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

namespace Touch.ViewModels
{
    /// <summary>
    ///     回忆列表ViewModel
    /// </summary>
    public class MemoryListViewModel : ViewModelBase
    {
        private RelayCommand<MemoryModel> _addCommand;
        private MemoryList _memoryList;
        
        public MemoryList MemoryList
        {
            get => _memoryList;
            set => Set(nameof(MemoryList), ref _memoryList, value);
        }
        private readonly IMemoryListService _memoryListService;
        private readonly IMemoryModelService _memoryModelService;
        public MemoryListViewModel(IMemoryListService memoryListService,
            IMemoryModelService memoryModelService)
        {
            _memoryListService = memoryListService;
            _memoryModelService = memoryModelService;
            MemoryList = new MemoryList();
        }


        /// <summary>
        ///     tipgrid是否显示
        /// </summary>
        public bool IsTipGridShow => MemoryList.MemoryModels.Count > 0;

//        /// <summary>
//        ///     最新key号
//        /// </summary>
//        public int LastKeyNo => MemoryList.LastKeyNo;

        public RelayCommand<MemoryModel> AddCommand =>
            _addCommand ?? (_addCommand = new RelayCommand<MemoryModel>(
                 memoryModel =>
                {
                    MemoryList = _memoryListService.Add(memoryModel, MemoryList);
                }));

        /// <summary>
        ///     删除点击操作
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                
                return new CommandHandler(memoryModel =>
                MemoryList = _memoryListService.Delete(memoryModel as MemoryModel, MemoryList)); }
        }
        public async Task<MemoryModel> CreateMemoryAsync(int lastKeyNo, String memoryName, List<ImageModel> imageModels)
        {
            MemoryModel memoryModel = await _memoryModelService.GetInstanceAsync(new MemoryModel());
            memoryModel = _memoryModelService.GetNewMemoryModel(memoryModel, lastKeyNo, memoryName, imageModels);
            return memoryModel;
        }
        /// <summary>
        ///     异步获取实例
        /// </summary>
        /// <returns></returns>
        public async Task GetInstanceAsync()
        {
            _memoryList = await _memoryListService.GetInstanceAsync();
        }
    }
}