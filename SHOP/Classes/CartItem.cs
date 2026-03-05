using CommunityToolkit.Mvvm.ComponentModel;

namespace SHOP.Classes;

//Primary constructor
public partial class CartItem(Products products, int count = 1) : ObservableObject
{
    
    public Products Products { get; set; } = products;

    [ObservableProperty] private int count = count;
    
}