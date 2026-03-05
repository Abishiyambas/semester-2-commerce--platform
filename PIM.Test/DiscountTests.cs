using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PIM_Logic;

namespace PIM.Test
{
    public class DiscountTests
    {
        private Pim pim;

        [SetUp]
        public async Task Setup()
        {
            AppEnvironment.IsTestEnvironment = true;
            pim = new Pim(new PIM_Data.DatabaseUtility () );
        }

        [Test]
        public async Task GetAllDiscountTest()
        {
            var allDiscounts = await pim.GetAllDiscounts();

            foreach (var d in allDiscounts)
            {
                if (d.Discount_id <= 0)
                    Assert.Fail("Discount_id is invalid");
                if (d.Discount_percentage <= 0 || d.Discount_percentage > 100)
                    Assert.Fail("Discount_percentage is wrong");
                if (d.Start_date == default)
                    Assert.Fail("Start_date is null or default");
                if (d.End_date == default)
                    Assert.Fail("End_date is null or default");
                if (!d.Product_id.HasValue && !d.Category_id.HasValue)
                    Assert.Fail("Product_id or Category_id are required");
            }
            Assert.Pass();
        }

        [Test]
        public async Task GetAllCategoryDiscountsTest()
        {
            var catDiscounts = await pim.GetAllCategoryDiscounts();

            foreach (var d in catDiscounts)
            {
                if (d.Discount_id <= 0)
                    Assert.Fail("Discount_id is invalid");
                if (d.Discount_percentage <= 0 || d.Discount_percentage > 100)
                    Assert.Fail("Discount_percentage is wrong");
                if (d.Start_date == default)
                    Assert.Fail("Start_date is null or default");
                if (d.End_date == default)
                    Assert.Fail("End_date is null or default");
                if (!d.Category_id.HasValue)
                    Assert.Fail("Category_id is null");
                if (d.Product_id.HasValue)
                    Assert.Fail("Product_id must be null for category discounts");
            }
            Assert.Pass();
        }

        [Test]
        public async Task GetAllProductDiscountsTest()
        {
            var productDiscounts = await pim.GetAllProductDiscounts();

            foreach (var d in productDiscounts)
            {
                if (d.Discount_id <= 0)
                    Assert.Fail("Discount_id is invalid");
                if (d.Discount_percentage <= 0 || d.Discount_percentage > 100)
                    Assert.Fail("Discount_percentage is wrong");
                if (d.Start_date == default)
                    Assert.Fail("Start_date is null or default");
                if (d.End_date == default)
                    Assert.Fail("End_date is null or default");
                if (!d.Product_id.HasValue)
                    Assert.Fail("Product_id is null");
                if (d.Category_id.HasValue)
                    Assert.Fail("Category_id must be null for product discounts");
            }
            Assert.Pass();
        }

        [Test]
        public async Task CreateDiscountTest()
        {
            HashSet<Discount> testDiscounts = GetTestDiscounts();
            foreach (Discount test in testDiscounts)
            {
                await pim.InsertDiscount(test);
                test.Discount_id = await pim.GetHighestDiscountId();
            }
            
            Assert.That( testDiscounts.IsSubsetOf ( (await pim.GetAllDiscounts()).ToHashSet() ), "Database does not contain the new discount set" );

            foreach (Discount test in testDiscounts)
            {
                await pim.DeleteDiscount(test);
            }
        }

        [Test]
        public async Task UpdateDiscountTest ()
        {
            var test = (GetTestDiscounts()).First();    // Create mock discount 
            test = await pim.InsertDiscount ( test );      // insert and update mock obj to contain id from db
            var newPct = test.Discount_percentage + 5;  // update discount 
            test.Discount_percentage = newPct;          // set update

            await pim.UpdateDiscount ( test );             // update discount in database


            Discount fetched;
            if (test.Product_id.HasValue)
                fetched = (await pim.GetDiscounts(test.Product_id.Value)).First(d => d.Discount_id == test.Discount_id);
            else
                fetched = (await pim.GetCategoryDiscount(test.Category_id.Value)).First(d => d.Discount_id == test.Discount_id);

            Assert.That(fetched.Discount_percentage, Is.EqualTo(newPct), "Update did not persist Discount_percentage");

            await pim.DeleteDiscount(test);
        }

        [Test]
        public async Task DeleteDiscountTest()
        {
            var test = (GetTestDiscounts()).First();
            await pim.InsertDiscount(test);

            await pim.DeleteDiscount(test);

            Discount[] fetched;
            if (test.Product_id.HasValue)
                fetched = await pim.GetDiscounts(test.Product_id.Value);
            else
                fetched = await pim.GetCategoryDiscount(test.Category_id.Value);

            Assert.That(fetched.Any(d => d.Discount_id == test.Discount_id), Is.False, "DeleteDiscount should remove the entry");
        }

        private HashSet<Discount> GetTestDiscounts()
        {
            return
            [
                new Discount(
                    20,
                    new DateTime(2025, 1, 1),
                    new DateTime(2025, 1, 31),
                    1,
                    null
                ),
                new Discount(
                    30,
                    new DateTime(2025, 2, 1),
                    new DateTime(2025, 2, 28),
                    null,
                    1
                )
            ];
        }

        [Test]
        public void IsActive_ReturnsTrue_WhenNowWithinRange()
        {
            var now = DateTime.Now;
            var discount = new Discount(
                50,
                now.AddMinutes(-1),
                now.AddMinutes(1),
                null,
                1
            );
            Assert.That(discount.IsActive(), Is.True, "Discount should be active when current time is within start and end dates");
        }

        [Test]
        public void IsActive_ReturnsFalse_WhenBeforeStart()
        {
            var now = DateTime.Now;
            var discount = new Discount(
                50,
                now.AddMinutes(1),
                now.AddMinutes(2),
                1,
                null
            );
            Assert.That(discount.IsActive(), Is.False, "Discount should not be active before the start date");
        }

        [Test]
        public void IsActive_ReturnsFalse_WhenAfterEnd()
        {
            var now = DateTime.Now;
            var discount = new Discount(
                50,
                now.AddMinutes(-2),
                now.AddMinutes(-1),
                1,
                null
            );
            Assert.That(discount.IsActive(), Is.False, "Discount should not be active after the end date");
        }
    }
}
