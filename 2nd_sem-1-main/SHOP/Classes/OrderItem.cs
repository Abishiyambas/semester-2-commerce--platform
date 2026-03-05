using System;

namespace SHOP.Classes;

public class OrderItem
{
    public Products Product { get; set; }
    public int Amount { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderDateOnly => OrderDate.ToShortDateString();
    public OrderItem(Products product, int amount, DateTime orderDate)
    {
        Product = product;
        Amount = amount;
        OrderDate = orderDate;
    }
}