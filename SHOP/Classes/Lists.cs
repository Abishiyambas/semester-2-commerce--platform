using System;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SHOP.Classes;

namespace SHOP
{
    //Singleton class to initialize Lists and load data
    public class ListInitializer
    {
        public static ListInitializer Instance { get; } = new ListInitializer ( /*true if local database*/ );
        private PIM_SHOP_Interface.IPim iPim;


        public ObservableCollection<Products> ProductsList { get; } = new ();
        public ObservableCollection<Products> DesktopList { get; } = new ();
        public ObservableCollection<Products> LaptopList { get; } = new ();
        public ObservableCollection<Products> HardwareList { get; } = new ();
        public ObservableCollection<Products> AccessoriesList { get; } = new ();
        public ObservableCollection<CartItem> CartItemList { get; } = new ();
        
        public ObservableCollection<Review> ReviewList { get; } = new();
        public ObservableCollection<Products> FilteredProductsList { get; set; } = new(); 
        
        public ObservableCollection<CartItem> OrderHistoryList { get; } = new();


        // wow Singleton nice job :) 
        private ListInitializer ( bool localDatabase = false )
        {
            if ( !localDatabase )
                iPim = new PIM_Logic.Pim ( new PIM_Data.DatabaseUtility () );

            LoadProductsFromDatabase ( localDatabase );
            CartItemList.Clear ();
        }

        public async void LoadProductsFromDatabase ( bool localDatabase)
        {
            // load mock data from shop local database
            if ( localDatabase )
            {
                SetupLocalDatabase ();
            }
            else
            {
                await LoadPimDatabase ();
        }
        }
        //PIMINTTEST
        private void SetupLocalDatabase ()
        {
            Credentials.Credentials credentials = new();
            string conString = credentials.Database;

            using ( var con = new NpgsqlConnection ( conString ) )
            {
                var data = con.Query<Products>("SELECT * FROM products").ToList();
                foreach ( var product in data )
                {
                    ProductsList.Add ( product );
                    if ( product.Type == "desktop" )
                        DesktopList.Add ( product );
                    else if ( product.Type == "laptop" )
                        LaptopList.Add ( product );
                    else if ( product.Type == "hardware" )
                        HardwareList.Add ( product );
                    else if ( product.Type == "accessories" )
                        AccessoriesList.Add ( product );
                }
            }
        }

        private async Task LoadPimDatabase ()
        {
            int[] ids = await iPim.GetAllProductIds();
            List<Products> products = new List<Products>();

            foreach ( int id in ids )
            {
                Task<string> name = iPim.GetProductName( id );
                Task<string> supplier = iPim.GetProductSupplier ( id );
                Task<string> sku = iPim.GetProductSKU ( id );
                Task<double> price = iPim.GetProductPrice ( id );
                Task<string> category = iPim.GetProductCategory ( id );
                Task<string> description = iPim.GetProductDescription ( id );
                Task<string> filePath = GetProductFilepath(id); 

                Task[] tasks =
                [
                    name,
                    supplier,
                    sku,
                    price,
                    category,
                    description,
                    filePath
                ];
                await Task.WhenAll ( tasks );

                products.Add (
                    new Products()
                    {
                        ID= id,
                        Name=  await name,
                        Manufacturer = await supplier,
                        EAN_Number = await sku,
                        Price = await price,
                        Type  = await category,
                        DescriptionPath  = await description,
                        FilePath = await filePath
                    }
                );
            }

            foreach ( var product in products )
            {
                ProductsList.Add ( product );
                if ( product.Type.ToLower () == "desktops" )
                    DesktopList.Add ( product );
                else if ( product.Type.ToLower () == "laptops" )
                    LaptopList.Add ( product );
                else if ( product.Type.ToLower () == "hardware" )
                    HardwareList.Add ( product );
                else if ( product.Type.ToLower () == "accessories" )
                    AccessoriesList.Add ( product );
            }
        }
        
        // method for loading pictures from our database and combining it with pim
        async public Task<string> GetProductFilepath (int productId)
        {
            // try's to connect to our database and finds the product that matches the productId. If the product Id matches an existing product, it returns its filepath.
            try
            {
                Credentials.Credentials credentials = new();
                string conString = credentials.Database;
                string query = "SELECT * FROM Products WHERE id = @id";
                using (var con = new NpgsqlConnection(conString))
                {
                    Products product = await con.QuerySingleAsync<Products>(query, new { id = productId });
                    return product.FilePath ?? throw new Exception("Product not found");
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting filepath: {ex.Message}" );
                return null;
            }
        }
    }
}