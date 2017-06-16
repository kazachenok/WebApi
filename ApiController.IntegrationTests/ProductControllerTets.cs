using BusinessServices;
using DataModel.Models;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using TestsHelper;


namespace ApiController.Tests
{
    public class ProductControllerTest
    {
        #region fields

        private IProductServices productService;
        private IUnitOfWork unitOfWork;

        private HttpResponseMessage response;
        private const string ServiceBaseURL = "http://localhost:50875/";

        #endregion

        #region Tests setup

        [OneTimeSetUp]
        public void Setup()
        {
            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseAlways<DataModelContext>());

            unitOfWork = new UnitOfWork();
            productService = new ProductServices(unitOfWork);
        }

        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            productService = null;
            unitOfWork = null;
            if (response != null)
                response.Dispose();
        }

        [SetUp]
        public void ReInitializeTest()
        {

        }

        [TearDown]
        public void DisposeTest()
        {
            if (response != null)
                response.Dispose();
        }

        #endregion

        #region Tests

        private ProductsController ControllerCreater(string relUri, HttpMethod method)
        {
            var productController = new ProductsController(productService)
            {
                Request = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(ServiceBaseURL + relUri)
                }
            };

            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return productController;
        }

        [Test]
        public async Task GetAllProductsTest()
        {
            var productController = ControllerCreater("api/Products", HttpMethod.Get);
            response = await productController.Get();

            var responseResult = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
            var comparer = new ProductComparer();
            //CollectionAssert.AreEqual(
            //responseResult.OrderBy(product => product, comparer),
        }

        #endregion
    }
}
