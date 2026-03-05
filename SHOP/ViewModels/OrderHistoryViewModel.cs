using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels;

public class OrderHistoryViewModel : ViewModelBase
{
    public RelayCommand GoToHomeCommand { get; }
    public Order order { get; }
    public ObservableCollection<OrderItem> OrderHistoryList { get; set; } = new();

    public OrderHistoryViewModel(User user, MainViewModel mainViewModel)
    {
        order = new Order(PrivateCustomer.LoggedInCustomer, PrivateCustomer.LoggedInCustomer.Address,
            PrivateCustomer.LoggedInCustomer.Zip);
        GoToHomeCommand = new RelayCommand(() => mainViewModel.GoToHome());

        LoadOrderHistory(user);
    }

    public async Task LoadOrderHistory(User user)
    {
        var history = await order.GetOrderHistory(user);
        OrderHistoryList.Clear();
        foreach (var item in history)
            OrderHistoryList.Add(item);
        OnPropertyChanged(nameof(OrderHistoryList));
    }
}
