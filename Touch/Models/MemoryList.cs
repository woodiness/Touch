using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using GalaSoft.MvvmLight;
using Touch.Data;

namespace Touch.Models
{
    /// <summary>
    ///     回忆列表
    /// </summary>
    public class MemoryList : ObservableObject
    {
        /// <summary>
        ///     数据库集合
        /// </summary>
        private DatabaseHelper _databaseHelper;

        public DatabaseHelper DatabaseHelper
        {
            get => _databaseHelper;
            set => Set(nameof(DatabaseHelper), ref _databaseHelper, value);
        }

        /// <summary>
        ///     回忆list
        /// </summary>
        private ObservableCollection<MemoryModel> _memoryModels;

        public ObservableCollection<MemoryModel> MemoryModels
        {
            get => _memoryModels;
            set => Set(nameof(MemoryModels), ref _memoryModels, value);
        }

        public MemoryList()
        {
            _databaseHelper = DatabaseHelper.GetInstance();
            _lastKeyNo = _databaseHelper.MemoryListDatabase.GetLastKeyNo();
            MemoryModels = new ObservableCollection<MemoryModel>();
        }

        /// <summary>
        ///     最新key号
        /// </summary>
        private int _lastKeyNo;
        public int LastKeyNo
        {
            get => _lastKeyNo = _databaseHelper.MemoryListDatabase.GetLastKeyNo();
            set => Set(nameof(LastKeyNo), ref _lastKeyNo, value);
        }
    }


}