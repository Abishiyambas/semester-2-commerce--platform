using Avalonia.Controls;
using System;

using PIM_Logic;
namespace PIM_Presentation;

public partial class MainTabControl : UserControl
{
    private readonly Pim pim;
    private readonly EmployeeManager employeeManager;

    private readonly UserControl productsPage;
    private readonly UserControl employeesPage;
    private readonly UserControl categoriespage;
    private readonly UserControl suppliersPage;
    private readonly UserControl discountsPage;
    private readonly UserControl statisticsPage;
    private readonly UserControl logoutPage;
    
    public UserControl ProductsPage => productsPage;
    public UserControl EmployeesPage => employeesPage;
    public UserControl CategoriesPage => categoriespage;
    public UserControl SuppliersPage => suppliersPage;
    public UserControl DiscountsPage => discountsPage;
    public UserControl StatisticsPage => statisticsPage;
    public UserControl LogoutPage => logoutPage;

    public MainTabControl ( Action<Control> setContentAction, Pim pim, EmployeeManager dbManager )
    {
        this.pim = pim;
        employeeManager = dbManager;
        DataContext = this;

        productsPage = new ProductsPage ( pim );
        employeesPage = GetEmployeePage ( pim, employeeManager );
        categoriespage = new CategoriesPage ( pim );
        suppliersPage = new SuppliersPage ( pim );
        discountsPage = new DiscountsPage(pim);
        statisticsPage = new StatisticsPage ( pim );
        logoutPage = new LogoutPage ( setContentAction, new LogInPage ( setContentAction, pim, dbManager ) );

        InitializeComponent ();
    }

    public static UserControl GetEmployeePage( Pim pim, EmployeeManager employeeManager )
    {
        if ( EmployeeManager.ActiveEmployee == null )
            return new UnauthorizedPage ();

        return EmployeeManager.ActiveEmployee.Authorized ? new EmployeesPage ( pim, employeeManager ) : new UnauthorizedPage ();
    }
}