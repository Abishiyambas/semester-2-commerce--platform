using NUnit.Framework;
using SHOP.ViewModels;
using SHOP.Classes;
using System;
using System.Reflection;

namespace SHOP.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        private MainViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            MainViewModel.ResetInstance();
            _viewModel = new MainViewModel();

        }

        [Test]
        public void HomePageConstructor()
        {
            Assert.That(_viewModel.CurrentPage, Is.TypeOf<HomePageViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }

        [Test]
        public void GoToLaptopCommand()
        {
            _viewModel.GoToLaptopCommand.Execute(null);
            Assert.That(_viewModel.CurrentPage, Is.TypeOf<LaptopViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }

        [Test]
        public void GoToAccessoriesCommand()
        {
            _viewModel.GoToAccessoriesCommand.Execute(null);
            Assert.That(_viewModel.CurrentPage, Is.TypeOf<AccessoriesViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }

        [Test]
        public void GoToCartCommand()
        {
            _viewModel.GoToCartCommand.Execute(null);
            Assert.That(_viewModel.CurrentPage, Is.TypeOf<CartViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }

        [Test]
        public void GoToProductDetailPageCommand()
        {
            var product = new Products { Name = "Example Product" };
            _viewModel.GoToProductDetailPageCommand.Execute(product);

            Assert.That(_viewModel.CurrentPage, Is.TypeOf<ProductDetailPageViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }

        [Test]
        public void HandleLoginSuccess()
        {
            var customer = new PrivateCustomer { Name = "TestUser" };

            // Use reflection to call private method
            var method = typeof(MainViewModel).GetMethod("HandleLoginSuccess", BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(_viewModel, new object[] { customer });

            Assert.That(_viewModel.LoggedInUsername, Is.EqualTo("TestUser"));
            Assert.That(_viewModel.UserLoggedIn, Is.True);
        }

        [Test]
        public void InstanceSingleton()
        {
            var instance1 = MainViewModel.Instance;
            var instance2 = MainViewModel.Instance;

            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void GoToHelpCommand()
        {
            _viewModel.GoToHelpCommand.Execute(null);
            Assert.That(_viewModel.CurrentPage, Is.TypeOf<HelpViewModel>());
            Console.WriteLine (_viewModel.CurrentPage);
        }
        
        [Test]
        public void TotalZero()
        {
            Assert.That(_viewModel.Total, Is.EqualTo(0.0));
            Console.WriteLine (_viewModel.Total);
        }

        [Test]
        public void ProductCounterZero()
        {
            Assert.That(_viewModel.ProductCounter, Is.EqualTo(0.0));
            Console.WriteLine (_viewModel.ProductCounter);
        }

        [Test]
        public void SettingTotal()
        {
            _viewModel.Total = 149.99;
            Assert.That(_viewModel.Total, Is.EqualTo(149.99));
            Console.WriteLine (_viewModel.Total);
        }

        [Test]
        public void SettingProductCounter()
        {
            _viewModel.ProductCounter = 3;
            Assert.That(_viewModel.ProductCounter, Is.EqualTo(3));
            Console.WriteLine (_viewModel.ProductCounter);
        }
        
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }

    }
}
