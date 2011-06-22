using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Husk.Model;
using System.IO;

namespace MVC_Husk.MsTests.Specs
{
    [TestClass]
    public class SeasonalProductsSpecs
    {
        SeasonalProducts _products;

        public SeasonalProductsSpecs()
        {
            _products = new SeasonalProducts();
        }

        [TestInitialize]
        public void Init()
        {
            _products.Delete(where: "1=@0", args: 1);
        }


        [TestMethod]
        public void create_seasonal_requires_date_be_valid()
        {
            var result = _products.CreateSeasonalProduct("1234", "SomeCategory", "1.124");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_require_date_be_in_the_past()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(1).ToShortDateString(), "SomeCategory", "1.124");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_require_date_be_greater_than_min_date()
        {
            var result = _products.CreateSeasonalProduct(DateTime.MinValue.ToShortDateString(), "SomeCategory", "1.124");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_require_category_not_be_null()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "", "1.124");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_require_seasonality_index_not_be_null()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_require_seasonality_index_be_a_double()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "NotADouble");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", result.Message);
        }

        [TestMethod]
        public void create_seasonal_adds_a_product_with_valid_data()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.2345");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Seasonal Product added", result.Message);
        }

        [TestMethod]
        public void creating_a_duplicate_seasonal_will_fail()
        {
            _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.2345");
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.2345");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("An entry for Cat for the week of " + DateTime.Today.AddDays(-1).ToShortDateString() + " already exists", result.Message);
        }

        [TestMethod]
        public void update_seasonal_requires_date_be_valid()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), "1234", "SomeCategory", "1.124");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_require_date_be_in_the_past()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(1).ToShortDateString(), "SomeCategory", "1.124");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_require_date_be_greater_than_min_date()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.MinValue.ToShortDateString(), "SomeCategory", "1.124");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_require_category_not_be_null()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(-1).ToShortDateString(), "", "1.124");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_require_seasonality_index_not_be_null()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_require_seasonality_index_be_a_double()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "NotADouble");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("The Week, Category or Seasonal Index was invalid", update.Message);
        }

        [TestMethod]
        public void update_seasonal_adds_a_product_with_valid_data()
        {
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.124");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.5432");

            Assert.IsTrue(update.Success);
            Assert.AreEqual("Seasonal Product Updated", update.Message);
        }

        [TestMethod]
        public void updating_a_record_to_create_a_duplicate_seasonal_will_fail()
        {
            _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.124");
            var result = _products.CreateSeasonalProduct(DateTime.Today.AddDays(-1).ToShortDateString(), "SomeCategory", "1.1334");
            var update = _products.UpdateSeasonalProduct(Convert.ToInt64(result.SeasonalProductId), DateTime.Today.AddDays(-1).ToShortDateString(), "Cat", "1.5432");

            Assert.IsFalse(update.Success);
            Assert.AreEqual("An entry for Cat for the week of " + DateTime.Today.AddDays(-1).ToShortDateString() + " already exists", update.Message);
        }

        // This is a pretty horrible test for the upload
        [TestMethod]
        public void upload_file_should_uploaded_valid_data()
        {
            var path = Path.GetFullPath("../../../MVC_Husk.MsTests/App_Data/Files/seasonality_short.txt");

            _products.LoadFileData(path);
            Assert.IsTrue(_products.All().Count() > 0);

        }
    }
}
