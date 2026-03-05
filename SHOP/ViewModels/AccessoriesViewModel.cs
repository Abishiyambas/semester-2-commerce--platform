using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public class AccessoriesViewModel : ViewModelBase
    {
        public ObservableCollection<Products> AccessoriesList => ListInitializer.Instance.AccessoriesList;
        
        public IRelayCommand<Products> GoToProductDetailPageCommand { get; }

        public AccessoriesViewModel(MainViewModel mainVm)
        {
            var mainVm1 = mainVm;
            GoToProductDetailPageCommand = new RelayCommand<Products>(product =>
            {
                if (product != null)
                {
                    mainVm1.GoToProductDetailPage(product);
                }
            });
        }
    }
}