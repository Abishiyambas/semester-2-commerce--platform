using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Dapper;
using Npgsql;
using SHOP.ViewModels;

namespace SHOP.Views
{
    public partial class RegisterUserView : UserControl
    {
        private RegisterUserViewModel? ViewModel => this.DataContext as RegisterUserViewModel;
        PrivateCustomer user = new PrivateCustomer();

        public RegisterUserView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void Register_OnClick(object? sender, RoutedEventArgs e)
        {
            string selectedCountry = Country.SelectionBoxItem.ToString();
            bool hasEmptyFields = false;
            bool hasRequired = false;

            if (EmptyFields(hasEmptyFields, selectedCountry)) return;
            if (RequiredFields(hasRequired, selectedCountry)) return;
            if (ZipCheck()) return;

            // Fortsæt registrering
            var customer = new PrivateCustomer();
            var result = customer.Register(
                name.Text,
                Email.Text,
                Password.Text,
                Address.Text,
                PostalCode.Text,
                City.Text,
                selectedCountry,
                Telephone.Text
            );

            if (result != null)
            {
                RegisterLabel.Content = "Registration Successful";
                Console.WriteLine("Registration successful!");
            }
            else
            {
                Console.WriteLine("Registration failed.");
                Email.Text = "User with this email already exists";
            }
        }

        private bool EmptyFields(bool hasEmptyFields, string? selectedCountry)
        {
            // Tjek for tomme felter
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                name.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                Email.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.Text = string.Empty;
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(Address.Text))
            {
                Address.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(PostalCode.Text))
            {
                PostalCode.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(City.Text))
            {
                City.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(selectedCountry))
            {
                Country.SelectedIndex = 0;
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(Telephone.Text))
            {
                Telephone.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(ConfirmE.Text))
            {
                ConfirmE.Text = "required";
                hasEmptyFields = true;
            }
            if (string.IsNullOrWhiteSpace(ConfirmP.Text))
            {
                ConfirmP.Text = string.Empty;
                hasEmptyFields = true;
            }
            if (Email.Text != ConfirmE.Text)
            {
                ConfirmE.Text = "Email does not match";
                hasEmptyFields = true;
            }
            if (Password.Text != ConfirmP.Text)
            {
                ConfirmP.Text = string.Empty;
                hasEmptyFields = true;
            }
            if (hasEmptyFields)
            {
                Console.WriteLine("Please fill in all required fields.");
                RegisterLabel.Content = "Registration Failed, please try again";
                return true;
            }

            return false;
        }

        private bool RequiredFields(bool hasRequired, string? selectedCountry)
        {
            // Tjek for nødvendige felter
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                name.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                Email.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                Password.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(Address.Text))
            {
                Address.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(PostalCode.Text))
            {
                PostalCode.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(City.Text))
            {
                City.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(selectedCountry))
            {
                Country.SelectedIndex = 0;
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(Telephone.Text))
            {
                Telephone.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(ConfirmE.Text))
            {
                ConfirmE.Text = "required";
                hasRequired = true;
            }
            if (string.IsNullOrWhiteSpace(ConfirmP.Text))
            {
                ConfirmP.Text = "required";
                hasRequired = true;
            }
            if (Email.Text != ConfirmE.Text)
            {
                ConfirmE.Text = "Email does not match";
                hasRequired = true;
            }
            if (Password.Text != ConfirmP.Text)
            {
                ConfirmP.Text = "Password does not match";
                hasRequired = true;
            }
            if (hasRequired)
            {
                Console.WriteLine("Please fill in all required fields.");
                RegisterLabel.Content = "Registration Failed, please try again";
                return true;
            }

            return false;
        }

        private bool ZipCheck()
        {
            // Tjek om zip og city findes i databasen
            using var con = new NpgsqlConnection(new Credentials.Credentials().Database);
            con.Open();

            string zipCityQuery = "SELECT COUNT(1) FROM zip WHERE zip = @Zip AND city = @City";
            int count = con.ExecuteScalar<int>(zipCityQuery, new { Zip = PostalCode.Text, City = City.Text });

            if (count == 0)
            {
                PostalCode.Text = "Postal or City is invalid";
                City.Text = "";
                Console.WriteLine("Zip and City combination not found in database.");
                return true;
            }

            return false;
        }

        private void ClearOnFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Clear();
            }
        }
    }
}
