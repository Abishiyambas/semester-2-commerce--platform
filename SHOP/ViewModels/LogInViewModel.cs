using SHOP.Classes;
using CommunityToolkit.Mvvm.Input;

namespace SHOP.ViewModels;

public class  LogInViewModel : ViewModelBase
{
    public RelayCommand GoToHomeCommand { get; }
    public IRelayCommand goToRegisterUserPageCommand { get; }

    public LogInViewModel(MainViewModel mainViewModel)
    {
        var mainViewModel1 = mainViewModel;
        GoToHomeCommand = new RelayCommand(() => mainViewModel1.GoToHome());
        goToRegisterUserPageCommand = new RelayCommand(() => mainViewModel1.GoToRegister());
    }
}