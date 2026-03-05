using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public class LaptopViewModel : ViewModelBase
    {
        public ObservableCollection<Products> LaptopList => ListInitializer.Instance.LaptopList;
        
        public IRelayCommand<Products> GoToProductDetailPageCommand { get; }

        public LaptopViewModel(MainViewModel mainVm)
        {
            var mainVm1 = mainVm;
            GoToProductDetailPageCommand = new RelayCommand<Products>(product =>
            {
                if (product != null)
                {
                    mainVm1.GoToProductDetailPage(product); // Gå til produktdetaljesiden med produktet
                }
            });
        }
    }
}