using CommunityToolkit.Mvvm.Input;

namespace SHOP.ViewModels;

public class RegisterUserViewModel : ViewModelBase
{
    private readonly MainViewModel mainViewModel;
    public RegisterUserViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
        
        
    }
}