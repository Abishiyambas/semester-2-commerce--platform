using Avalonia.Controls;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using OMS.Logic;
using System;
using System.Collections.Generic;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using System.Linq;

namespace OMS.Presentation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //If there is not made an employe table in the database, with username and password, change Maintab.IsEnabled to true.
        MainTab.IsEnabled = false;

    }

    private void AddToOrderList(Order order)
    {
        Control[] dataPoints =
        {
            AddControlInBorder(new TextBlock { Text = order.GetOrderId.ToString() }),
            AddControlInBorder(new TextBlock { Text = order.GetCustomerId.ToString() }),
            AddControlInBorder(new TextBlock { Text = order.GetProductId.ToString() }),
            AddControlInBorder(new TextBlock { Text = order.GetAmount.ToString() }),
            AddControlInBorder(new TextBlock { Text = order.GetPrice.ToString() }),
            AddControlInBorder(new TextBlock { Text = order.GetCurrentStatus }),
            AddControlInBorder(new TextBlock { Text = order.GetOrderDate.ToString() })
        };

        var gridIndex = OrderGrid.RowDefinitions.Count;
        OrderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        for (int i = 0; i < dataPoints.Length; i++)
        {
            Control control = dataPoints[i];
            control.VerticalAlignment = VerticalAlignment.Bottom;
            OrderGrid.Children.Add(control);
            Grid.SetRow(control, gridIndex);
            Grid.SetColumn(control, i);
        }
    }

    private void ClearOrderList()
    {
        // Remove all rows except the header.
        OrderGrid.Children.Clear();
        OrderGrid.RowDefinitions.Clear();
    }

    private Border AddControlInBorder(Control control)
    {
        Border border = new Border()
        {
            BorderBrush = Brushes.DarkOrange,
            BorderThickness = new Thickness(0, 0, 0, 1),
            Child = control,
        };
        return border;

    }

    private void LoadOrderList()
    {
        // Adds to the orderlist.
        foreach (Order o in Order.GetOrderList())
        {
            Order.GetOrderList();
            AddToOrderList(o);
        }
        
    }

    //refresh button
    private void referesh_button(object? sender, RoutedEventArgs e)
    {

        ClearOrderList();
        LoadOrderList();
    }

    //sortbyprice high to low
    private void sortbyprice_hightolow(object? sender, RoutedEventArgs e)
    {
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Price.SortByPrice_High_To_Low(o_);

        ClearOrderList();
        

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);
        }
    }
   

    //sort by price low to high
    private void Sortbyprice_lowtohigh(object? sender, RoutedEventArgs e)
    {
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Price.SortByPrice_Low_To_High(o_);

        
        ClearOrderList();

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);

        }
    }

    //searchbox
    private void Searchbox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            List<Order> o_ = new();
            List<Order> searched_orders = new();
            int searched_order = int.Parse(searchbox.Text);

            o_ = Order.GetOrderList();
            ;

           searched_orders = Search_Box.searched_orders(o_, searched_order);


            ClearOrderList();
            
            foreach (Order o in searched_orders)
            {
                AddToOrderList(o);
            }

        }
    }


    private void sortbyamount_Large(object? sender, RoutedEventArgs e)
    {
     
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Amount.SortByAmount_High_To_low(o_);

        
        ClearOrderList();

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);

        }
        
    }

    private void sortbyamount_low(object? sender, RoutedEventArgs e)
    {
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Amount.SortByAmount_Low_To_High(o_);

        
        ClearOrderList();

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);

        }
    }

    private void SortByDate_NewestFirst(object? sender, RoutedEventArgs e)
    {
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Date.SortByDate_Newest_First(o_);

        
        ClearOrderList();

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);

        }    
    }

    private void SortByDate_OldestFirst(object? sender, RoutedEventArgs e)
    {
        List<Order> o_ = new();
        List<Order> sortedorders = new();

        o_ = Order.GetOrderList();

        sortedorders = Sort_By_Date.SortByDate_Oldest_First(o_);

        
        ClearOrderList();

        foreach (Order o in sortedorders)
        {
            AddToOrderList(o);

        }
        
    }

   
    private void login_Button(object? sender, RoutedEventArgs e)
    {
       string username = UsernameBox.Text;
       string password = PasswordBox.Text;

        if (ValidateUser.Validater(username, password))
        {
            LoginStatus.Text = "Login successful!";
            MainTab.IsEnabled = true;
            MainTabControl.SelectedItem = MainTab;
            

            MainTabControl.Items.Remove(LoginTab);

        }
        else
        {
            LoginStatus.Text = $"Invalid username: {username} or password: {password}";
        
        }    
        
        
    }

   
}
   

   

  
