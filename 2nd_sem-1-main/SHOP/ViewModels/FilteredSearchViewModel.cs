using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels;

public class FilteredSearchViewModel : ViewModelBase
{
    public ObservableCollection<Products> FilteredProducts { get; set; }
    
    public IRelayCommand<Products> GoToProductDetailPageCommand { get; }
    public FilteredSearchViewModel(MainViewModel mainVm)
    {
        FilteredProducts = MainViewModel.Instance.FilteredProductsList;
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