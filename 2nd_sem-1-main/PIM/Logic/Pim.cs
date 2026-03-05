using PIM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIM_Logic
{
    public partial class Pim
    {
        private readonly PIM_Data.IOperations dbOperations;

        public Pim ( PIM_Data.IOperations dbOperations )
        {
            DatabaseUtility.ExecuteSqlFile ();
            this.dbOperations = dbOperations;
        }

        public async Task<Product?> GetProduct ( int productId )
        {
            var product = await dbOperations.SelectSingle("products", "product_id", productId, InstantiateProduct);

            if ( product != null )
            {
                Task[] tasks = await GetProductTasks(product);

                // Set the result using setter method
                product.SetCategory ( await ( (Task<Category?>) tasks[0] ) );
                product.SetSupplier ( await ( (Task<Supplier?>) tasks[1] ) );
                product.SetReviews ( await ( (Task<Review[]>) tasks[2] ) );
                product.SetDiscounts ( await ( (Task<Discount[]>) tasks[3] ) );
                product.SetPriceHistroy ( await ( (Task<PriceHistory[]>) tasks[4] ) );
            }
            return product;
        }

        public async Task<int?> GetProductId ( string productSKU )
        {
            Product? product = await dbOperations.SelectSingle ( "products", "sku", productSKU, InstantiateProduct );
            if ( product is null )
                return null;
            return product.Product_id;
        }

        public async Task<int?> GetCategoryId ( int productId )
        {
            var category = await dbOperations.SelectSingle("categories", "product_id", productId, InstantiateCategory);
            if ( category is null )
                return null;
            return category.Category_id;
        }

        public async Task<int?> GetCategoryId ( string categoryName )
        {
            var category = await dbOperations.SelectSingle ( "categories", "name", categoryName, InstantiateCategory );
            if ( category is null )
                return null;
            return category.Category_id;
        }

        public async Task<int?> GetSupplierId ( int productId )
        {
            var supplier = await dbOperations.SelectSingle ( "suppliers", "product_id", productId, InstantiateSupplier );
            if ( supplier is null )
                return null;
            return supplier.Supplier_id;
        }

        public async Task<int?> GetSupplierId ( string supplierName )
        {
            var supplier = await dbOperations.SelectSingle ( "suppliers", "name", supplierName, InstantiateSupplier );
            if ( supplier is null )
                return null;
            return supplier.Supplier_id;
        }

        public async Task<Supplier?> GetSupplier ( int supplierId )
        {
            return await dbOperations.SelectSingle ( "suppliers", "supplier_id", supplierId, InstantiateSupplier );
        }

        public async Task<Category?> GetCategory ( int? categoryId )
        {
            if ( categoryId == null )
                return null;

            return await dbOperations.SelectSingle ( "categories", "category_id", categoryId, InstantiateCategory );
        }

        public async Task<Employee?> GetEmployee ( int employeeId )
        {
            return await dbOperations.SelectSingle ( "employees", "employee_id", employeeId, InstantiateEmployee );
        }

        async public Task<Discount[]> GetDiscounts ( int productId )
        {
            return ( await dbOperations.SelectAll ( "discounts", "product_id", productId, InstantiateDiscounts ) ).ToArray ();
        }

        public async Task<Discount[]> GetCategoryDiscount ( int categoryId )
        {
            return ( await dbOperations.SelectAll ( "discounts", "category_id", categoryId, InstantiateDiscounts ) ).ToArray ();
        }

        async public Task<PriceHistory[]> GetPriceHistory ( int productId )
        {
            return ( await dbOperations.SelectAll ( "price_history", "product_id", productId, InstantiatePriceHistory ) ).ToArray ();
        }

        public async Task<Review[]> GetReviews ( int productId )
        {
            return ( await dbOperations.SelectAll ( "reviews", "product_id", productId, InstanctiateReview ) ).ToArray ();
        }

        public async Task<Product[]> GetAllProducts ()
        {
            var products = await dbOperations.SelectAll("products", InstantiateProduct);

            foreach ( var product in products )
            {
                Task[] tasks = await GetProductTasks(product);

                // Set the result using setter method
                product.SetCategory ( await ( (Task<Category?>) tasks[0] ) );
                product.SetSupplier ( await ( (Task<Supplier?>) tasks[1] ) );
                product.SetReviews ( await ( (Task<Review[]>) tasks[2] ) );
                product.SetDiscounts ( await ( (Task<Discount[]>) tasks[3] ) );
                product.SetPriceHistroy ( await ( (Task<PriceHistory[]>) tasks[4] ) );
            }
            return products.ToArray ();
        }

        public async Task<Supplier[]> GetAllSuppliers ()
        {
            return ( await dbOperations.SelectAll ( "suppliers", InstantiateSupplier ) ).ToArray ();
        }

        public async Task<Category[]> GetAllCategories ()
        {
            return ( await dbOperations.SelectAll ( "categories", InstantiateCategory ) ).ToArray ();
        }

        public async Task<Employee[]> GetAllEmployees ()
        {
            return ( await dbOperations.SelectAll ( "employees", InstantiateEmployee ) ).ToArray ();
        }

        public async Task<Discount[]> GetAllDiscounts ()
        {
            return ( await dbOperations.SelectAll ( "discounts", InstantiateDiscounts ) ).ToArray ();
        }

        public async Task<Discount[]> GetAllProductDiscounts ()
        {
            var all = await dbOperations.SelectAll ("discounts", InstantiateDiscounts);
            return all.Where ( d => d.Product_id != null ).ToArray ();
        }

        public async Task<Discount[]> GetAllCategoryDiscounts ()
        {
            var all = await dbOperations.SelectAll("discounts", InstantiateDiscounts);
            return all.Where ( d => d.Category_id != null ).ToArray ();
        }


        // -------------------- INSERT METHODS --------------------
        public async Task<Product> InsertProduct ( Product product )
        {
            try
            {
                return await dbOperations.InsertSelect ( "products", product, InstantiateProduct );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error inserting product: {e.Message}" );
            }
            return default;
        }

        public async Task InsertSupplier ( Supplier supplier )
        {
            try
            {
                await dbOperations.Insert ( "suppliers", supplier );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error inserting supplier: {e.Message}" );
            }
        }

        public async Task InsertCategory ( Category category )
        {
            await dbOperations.Insert ( "categories", category );
        }

        public async Task<Employee> InsertEmployee ( Employee employee )
        {
            try
            {
                return await dbOperations.InsertSelect ( "employees", employee, InstantiateEmployee );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error inserting employee: {e.Message}" );
            }
            return default;
        }

        public async Task<Discount> InsertDiscount ( Discount discount )
        {
            try
            {
                return await dbOperations.InsertSelect ( "discounts", discount, InstantiateDiscounts );
            }

            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error inserting discount: {e.Message}" );
            }
            return default;
        }

        public async Task InsertPriceHistory( PriceHistory priceHistory)
        {
            await dbOperations.Insert ( "price_history", priceHistory );
        }

        // -------------------- UPDATE METHODS --------------------
        public async Task UpdateProduct ( Product product )
        {
            try
            {
                await dbOperations.Update ( "products", "product_id", product.Product_id, product );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error updating product: {e.Message}" );
            }
        }

        public async Task UpdateCategory ( Category category )
        {
            await dbOperations.Update ( "categories", "category_id", category.Category_id, category );
        }

        public async Task UpdateSupplier ( Supplier supplier )
        {
            await dbOperations.Update ( "suppliers", "supplier_id", supplier.Supplier_id, supplier );
        }

        public async Task UpdateEmployee ( Employee employee )
        {
            try
            {
                await dbOperations.Update ( "employees", "employee_id", employee.Employee_id, employee );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error updating employee: {e.Message}" );
            }
        }

        public async Task UpdateDiscount ( Discount discount )
        {
            try
            {
                await dbOperations.Update ( "discounts", "discount_id", discount.Discount_id, discount );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error updating discount: {e.Message}" );
            }
        }

        public async Task UpdateStock ( Product product ) // This is just update product
        {
            try
            {
                await UpdateProduct ( product );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error updating stock: {e.Message}" );
            }
        }

        // -------------------- DELETE METHODS --------------------
        public async Task DeleteProduct ( int productId )
        {
            try
            {
                await dbOperations.Delete ( "products", "product_id", productId );
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine ( $"Error deleting product: {ex.Message}" );
            }
        }

        public async Task DeleteCategory ( int categoryId )
        {
            await dbOperations.Delete ( "categories", "category_id", categoryId );
        }

        public async Task DeleteSupplier ( int supplierId )
        {
            await dbOperations.Delete ( "suppliers", "supplier_id", supplierId );
        }

        public async Task DeleteEmployee ( Employee employee )
        {
            try
            {
                if ( EmployeeManager.ActiveEmployee == null )
                {
                    throw new Exception ( $"Only authorized employee can delete employees" );
                }

                if ( EmployeeManager.ActiveEmployee.Authorized )
                {
                    await dbOperations.Delete ( "employees", "employee_id", employee.Employee_id );
                }
                else
                    return;
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error deleting employee: {e.Message}" );
            }
        }

        public async Task DeleteDiscount ( Discount discount )
        {
            try
            {
                await dbOperations.Delete ( "discounts", "discount_id", discount.Discount_id );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( $"Error deleting discount: {e.Message}" );
            }
        }

        public async Task<int> GetHighestEmployeeId ()
        {
            return await dbOperations.Max ( "employees", "employee_id" );
        }

        public async Task<int> GetHighestDiscountId ()
        {
            return await dbOperations.Max ( "discounts", "discount_id" );
        }

        public async Task<int> GetHighestProductId ()
        {
            return await dbOperations.Max ( "products", "product_id" );
        }

        private Employee InstantiateEmployee ( Npgsql.NpgsqlDataReader reader )
        {
            return new Employee ( reader );
        }

        private Supplier InstantiateSupplier ( Npgsql.NpgsqlDataReader reader )
        {
            return new Supplier ( reader );
        }

        private Review InstanctiateReview ( Npgsql.NpgsqlDataReader reader )
        {
            return new Review ( reader );
        }

        private Product InstantiateProduct ( Npgsql.NpgsqlDataReader reader )
        {
            return new Product ( reader );
        }

        private Category InstantiateCategory ( Npgsql.NpgsqlDataReader reader )
        {
            return new Category ( reader );
        }

        private PriceHistory InstantiatePriceHistory ( Npgsql.NpgsqlDataReader reader )
        {
            return new PriceHistory ( reader );
        }

        private Discount InstantiateDiscounts ( Npgsql.NpgsqlDataReader reader )
        {
            return new Discount ( reader );
        }

        private async Task<Task[]> GetProductTasks ( Product product )
        {
            // Setup tasks for getting the properties
            Task<Category?> categoryTask = GetCategory(product.Category_id);
            Task<Supplier?> supplierTask = GetSupplier(product.Supplier_id);
            Task<Review[]> reviewsTask = GetReviews(product.Product_id);
            Task<Discount[]> discountsTask = GetDiscounts(product.Product_id);
            Task<PriceHistory[]> priceHistoryTask = GetPriceHistory(product.Product_id);

            Task[] tasks = [categoryTask, supplierTask, reviewsTask, discountsTask, priceHistoryTask];

            await Task.WhenAll ( tasks ); // Wait for all tasks to finish
            return tasks;
        }
    }
}