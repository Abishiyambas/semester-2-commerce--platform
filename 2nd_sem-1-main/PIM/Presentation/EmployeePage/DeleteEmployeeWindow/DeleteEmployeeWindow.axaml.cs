using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;

namespace PIM_Presentation
{
    public partial class DeleteEmployeeWindow : Window
    {
        private Pim pim;
        private Employee employee;
        public DeleteEmployeeWindow(Pim pim, Employee employee)
        {
            InitializeComponent ();
            this.pim = pim;
            this.employee = employee;

            
            NameTextBox.Text = employee.Name;
            IdTextBox.Text = employee.Employee_id.ToString ();
            AuthorizationTextBox.Text = employee.Authorized ? "Authorized" : "Not Authorized";
            
        }
        
        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
        
        public async void Delete_Click(object? sender, RoutedEventArgs e)
        {
            await pim.DeleteEmployee(employee);
            Close(true);
        }
    }
}