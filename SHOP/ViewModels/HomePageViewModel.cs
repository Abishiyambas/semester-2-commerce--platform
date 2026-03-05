using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ObservableCollection<Products> ProductsList => ListInitializer.Instance.ProductsList;
        
        public IRelayCommand<Products> GoToProductDetailPageCommand { get; }

        public HomePageViewModel(MainViewModel mainVm)
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