using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public class HardwareViewModel : ViewModelBase
    {
        public ObservableCollection<Products> HardwareList => ListInitializer.Instance.HardwareList;
        
        public IRelayCommand<Products> GoToProductDetailPageCommand { get; }

        public HardwareViewModel(MainViewModel mainVm)
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

