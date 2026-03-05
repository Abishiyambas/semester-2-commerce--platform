using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public class DesktopViewModel : ViewModelBase
    {
        public ObservableCollection<Products> DesktopList => ListInitializer.Instance.DesktopList;
        
        public IRelayCommand<Products> GoToProductDetailPageCommand { get; }

        public DesktopViewModel(MainViewModel mainVm)
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