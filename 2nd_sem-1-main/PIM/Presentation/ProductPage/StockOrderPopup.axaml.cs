using System;
using Avalonia.Controls;
using PIM_Logic;

namespace PIM_Presentation;

public partial class StockOrderPopup : Window
{
    private Pim pim;
    private Product product;
    
    private int _maxAmount;
    private int _currentAmount;
    private int _orderAmount;

    public event Action<int>? StockUpdated;
    public event Action? OrderCanceled;

    public StockOrderPopup(Pim pim, Product product)
    {
        InitializeComponent();
        
        this.product = product;
        this.pim = pim;

        _maxAmount = product.Max_stock;
        _currentAmount = product.Current_stock;
        _orderAmount = 0;
        
        MaxAmountBox.Text = _maxAmount.ToString();
        CurrentAmountBox.Text = _currentAmount.ToString();
        OrderAmountBox.Text = _orderAmount.ToString();
        UpdateAfterAmount();

        IncreaseButton.Click += (_, _) => ChangeOrderAmount(1);
        DecreaseButton.Click += (_, _) => ChangeOrderAmount(-1);
        MaxButton.Click += (_, _) => SetOrderToMax();
        ConfirmButton.Click += (_, _) => ConfirmOrder();
        CancelButton.Click += (_, _) => CancelOrder();
    }

    private void ChangeOrderAmount(int delta)
    {
        _orderAmount = Math.Clamp(_orderAmount + delta, 0, _maxAmount - _currentAmount);
        OrderAmountBox.Text = _orderAmount.ToString();
        UpdateAfterAmount();
    }

    private void SetOrderToMax()
    {
        _orderAmount = _maxAmount - _currentAmount;
        OrderAmountBox.Text = _orderAmount.ToString();
        UpdateAfterAmount();
    }

    private void UpdateAfterAmount()
    {
        int total = _currentAmount + _orderAmount;
        AfterAmountBox.Text = total.ToString();
    }

    private void ConfirmOrder()
    {
        StockUpdated?.Invoke(_orderAmount);
        pim.UpdateStock(product);
        Close();
    }

    private void CancelOrder()
    {
        OrderCanceled?.Invoke();
        Close();
    }
    
}