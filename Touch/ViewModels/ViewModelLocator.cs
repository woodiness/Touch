using GalaSoft.MvvmLight.Ioc;
using Touch.Services;
namespace Touch.ViewModels
{
    /// <summary>
    ///     ViewModel定位器。
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     ViewModel定位器单件。
        /// </summary>
        public static readonly ViewModelLocator Instance =
            new ViewModelLocator();

        /// <summary>
        ///     构造函数。
        /// </summary>
        private ViewModelLocator()
        {
            SimpleIoc.Default.Register<IFolderModelService, FolderModelService>();
            SimpleIoc.Default.Register<IFolderListService, FolderListService>();  
            SimpleIoc.Default.Register<IImageModelService,ImageModelService>();
            SimpleIoc.Default.Register<IImageFolderListService,ImageFolderListService>();
            SimpleIoc.Default.Register<IGalleryImageListService, GalleryImageListService>();
            SimpleIoc.Default.Register<IMemoryListService, MemoryListService>();
            SimpleIoc.Default.Register<IMemoryModelService,MemoryModelService>();
            SimpleIoc.Default.Register<FolderListViewModel>();
            SimpleIoc.Default.Register<GalleryImageListViewModel>();
            SimpleIoc.Default.Register<MemoryListViewModel>();
        }


        public FolderListViewModel FolderListViewModel =>
            SimpleIoc.Default.GetInstance<FolderListViewModel>();
        public GalleryImageListViewModel GalleryImageListViewModel =>
            SimpleIoc.Default.GetInstance<GalleryImageListViewModel>();
        public MemoryListViewModel MemoryListViewModel =>
            SimpleIoc.Default.GetInstance<MemoryListViewModel>();
    }
}