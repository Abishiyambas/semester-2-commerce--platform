using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia;
using Avalonia.Media;
using System.Linq;
using PIM_Logic;

namespace PIM_Presentation;

public partial class EmployeesPage : Page
{
    private Employee[] employeesInGrid;
    private EmployeeManager employeeManager;

    public EmployeesPage (Pim pim, EmployeeManager employeeManager )
    {
        InitializeComponent ();
        this.pim = pim;
        this.employeeManager = employeeManager;
        ActiveEmployeeTextBlock.Text = EmployeeManager.ActiveEmployee.Name != null ? $"Welcome {EmployeeManager.ActiveEmployee.Name}" : "";

        if ( !AppEnvironment.IsTestEnvironment )
        {
            employeesInGrid = null;
            RefreshButton.Content = ImageHandler ( "Refresh_icon" );
            LoadEmployees ();
        }
    }

    public async void AddEmployeeButtonHandler(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var addEmployeeWindow = new AddEmployeeWindow(pim, RefreshButtonHandler);
            var parentWindow = this.VisualRoot as Window;
            if (parentWindow != null)
            {
                await addEmployeeWindow.ShowDialog( parentWindow );
            }
        }
    }
    
    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (sender is TextBox searchBox)
        {
            LoadAllEmployeesIntoGrid(searchBox.Text);
        }
    }
    
    public void RefreshButtonHandler(object? sender, RoutedEventArgs args)
    {
        LoadEmployees ();
    }

    public async void LoadEmployees()
    {
        employeesInGrid = await pim.GetAllEmployees ();
        LoadAllEmployeesIntoGrid ();
    }

    private void LoadAllEmployeesIntoGrid(string searchParam = null)
    {
        EmployeeGrid.RowDefinitions.Clear();
        EmployeeGrid.Children.Clear();

        Employee[] employeesToDisplay;
        
        if (string.IsNullOrEmpty(searchParam))
        {
            employeesToDisplay = employeesInGrid;
        }
        else
        {
            employeesToDisplay= FilterEmployees(searchParam);  
        } 

        foreach (Employee employee in employeesToDisplay)
        {
            AddEmployeeToGrid(employee);
        }
    }

    private Employee[] FilterEmployees(string searchParam = null)
    {
        if (employeesInGrid == null) return Array.Empty<Employee>();
        
        return employeesInGrid.Where(employee => employee.Name.ToLower().Contains(searchParam) ||
                                                 employee.Employee_id.ToString().Contains(searchParam)).ToArray();
    }

    private void AddEmployeeToGrid(Employee employee)
    {
        Control[] parameters =
        {
            SetControlIntoBorder(new TextBlock { Text = employee.Name }),
            SetControlIntoBorder(new TextBlock { Text = employee.Employee_id.ToString() }),
            EditButton(employee),
            DeleteButton(employee)
        };

        int rowIndex = EmployeeGrid.RowDefinitions.Count;
        EmployeeGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        for (int i = 0; i < parameters.Length; i++)
        {
            Control control = parameters[i];
            EmployeeGrid.Children.Add(control);
            Grid.SetRow(control, rowIndex);
            Grid.SetColumn(control, i);
        }
    }
    
    private Border EditButton(Employee employee)
    {
        Button editButton = new Button
        {
            Padding = new Thickness(0),
            Background = Brushes.Transparent,
            Width = 25,
            Height = 25
        };

        editButton.Content = ImageHandler( "edit-icon" );

        ToolTip.SetTip(editButton, "Edit this employee");
        
        editButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if (parentWindow != null )
            {
                var editWindow = new EditEmployeeWindow(pim, employee);
                await editWindow.ShowDialog ( parentWindow );
                LoadEmployees ();
            }
        };

        return SetControlIntoBorder ( editButton );
    }

    public Border DeleteButton(Employee employee)
    {
        if (employee.Authorized)
        {
            Button disabledDeleteButton = new Button
            {
                Padding = new Thickness(0),
                Background = Brushes.Transparent,
                Width = 25,
                Height = 25,
                IsEnabled = false,
                Content = ImageHandler("Remove-icon")
            };

            // Wrap the disabled button in an enabled Border:
            Border container = new Border
            {
                Child = disabledDeleteButton,
                Background = Brushes.Transparent
            };

            // Set the tooltip on the container instead:
            ToolTip.SetTip(container, "Authorized employee cannot be deleted");
            return SetControlIntoBorder(container);
        }

        Button deleteButton = new Button
        {
            Padding = new Thickness(0),
            Background = Brushes.Transparent,
            Width = 25,
            Height = 25
        };

        ToolTip.SetTip(deleteButton, "Delete this employee");
        deleteButton.Content = ImageHandler("Remove-icon");

        deleteButton.Click += async (s, e) =>
        {
            await DeleteButtonHandler ( employee );
        };

        return SetControlIntoBorder(deleteButton);
    }

    private async System.Threading.Tasks.Task DeleteButtonHandler ( Employee employee )
    {
        var parentWindow = this.VisualRoot as Window;
        if ( parentWindow != null )
        {
            var deleteWindow = new DeleteEmployeeWindow(pim, employee);
            await deleteWindow.ShowDialog ( parentWindow );
            LoadEmployees ();
        }
    }
}