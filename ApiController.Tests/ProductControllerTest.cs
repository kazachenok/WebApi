using BusinessEntities;
using BusinessServices;
using DataModel.Models;
using DataModel.Repository;
using DataModel.UnitOfWork;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ProductService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private List<Product> products;
        private GenericRepository<Product> productRepository;
        private DataModelContext dataModelContext;

        private HttpResponseMessage response;
        private const string ServiceBaseURL = "http://localhost:50875/";

        #endregion

        #region Tests setup

        [OneTimeSetUp]
        public void Setup()
        {
            products = SetUpProducts();
            dataModelContext = new Mock<DataModelContext>().Object;
            productRepository = SetUpProductRepository();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.SetupGet(s => s.ProductRepository).Returns(productRepository);
            unitOfWork = unitOfWorkMock.Object;
            productService = new ProductServices(unitOfWork);
        }

        private static List<Product> SetUpProducts()
        {
            var prodId = new int();
            var products = DataInitializer.GetAllProducts();
            foreach (Product prod in products)
                prod.Id = ++prodId;
            return products;
        }

        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            productService = null;
            unitOfWork = null;
            productRepository = null;
            products = null;
            if (response != null)
                response.Dispose();
        }

        [SetUp]
        public void ReInitializeTest()
        {
            //products = SetUpProducts();
        }

        [TearDown]
        public void DisposeTest()
        {
            if (response != null)
                response.Dispose();
        }

        private GenericRepository<Product> SetUpProductRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<GenericRepository<Product>>(MockBehavior.Default, dataModelContext);

            // Setup mocking behavior
            mockRepo.Setup(p => p.GetAll()).Returns(products);

            mockRepo
                .Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, Product>(
                    id => products.Find(p => p.Id.Equals(id))));

            mockRepo
                .Setup(p => p.Insert((It.IsAny<Product>())))
                .Callback(new Action<Product>(newProduct =>
                {
                    dynamic maxProductID = products.Last().Id;
                    dynamic nextProductID = maxProductID + 1;
                    newProduct.Id = nextProductID;
                    products.Add(newProduct);
                }));

            mockRepo
                .Setup(p => p.Update(It.IsAny<Product>()))
                .Callback(new Action<Product>(prod =>
                {
                    var oldProduct = products.Find(a => a.Id == prod.Id);
                    oldProduct = prod;
                }));

            mockRepo
                .Setup(p => p.Delete(It.IsAny<Product>()))
                .Callback(new Action<Product>(prod =>
                {
                    var productToRemove =products.Find(a => a.Id == prod.Id);

                    if (productToRemove != null)
                        products.Remove(productToRemove);
                }));

            // Return mock implementation object
            return mockRepo.Object;
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
        public void GetAllProductsTest()
        {
            var productController = ControllerCreater("api/Products", HttpMethod.Get);
            
            response = productController.Get();
            var responseResult = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.Any(), true);
            var comparer = new ProductComparer();
            CollectionAssert.AreEqual(
            responseResult.OrderBy(product => product, comparer),
            products.OrderBy(product => product, comparer), comparer);
        }

        [Test]
        public void GetProductByIdTest()
        {
            int id = 2;
            var productController = ControllerCreater("api/Products/" + id,  HttpMethod.Get);
            response = productController.Get(id);
            var responseResult = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            AssertObjects.PropertyValuesAreEquals(responseResult, products.Find(a => a.Id == id));
        }

        [Test]
        public void GetProductByWrongIdTest()
        {
            var productController = ControllerCreater("api/Products/10", HttpMethod.Get);
            response = productController.Get(10);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void CreateProductTest()
        {
            var productController = ControllerCreater("api/Products", HttpMethod.Post);

            var newProduct = new ProductEntity()
            {
                Name = "Web Api Service",
                CreateDate = DateTime.Now.AddMinutes(-1),
                isActive = false
            };

            var maxProductIDBeforeAdd = products.Max(a => a.Id);
            newProduct.Id = maxProductIDBeforeAdd + 1;
            productController.Post(newProduct);
            var addedproduct = new Product() { Name = newProduct.Name, Id = newProduct.Id, CreateDate = newProduct.CreateDate, isActive = newProduct.isActive };
            AssertObjects.PropertyValuesAreEquals(addedproduct, products.Last());
            Assert.That(maxProductIDBeforeAdd + 1, Is.EqualTo(products.Last().Id));
        }

        [Test]
        public void UpdateProductTest()
        {
            var firstProduct = products.First();

            var productController = ControllerCreater("api/Products/" + firstProduct.Id, HttpMethod.Put);
           
            var updatedProduct = new ProductEntity() { Name = firstProduct.Name, Id = firstProduct.Id, CreateDate = firstProduct.CreateDate, isActive = firstProduct.isActive };
            updatedProduct.Name = "Web Api Service";
            productController.Put(firstProduct.Id, updatedProduct);
            Assert.That(firstProduct.Id, Is.EqualTo(updatedProduct.Id));
            Assert.That(firstProduct.Name, Is.EqualTo(updatedProduct.Name));
            Assert.That(firstProduct.CreateDate, Is.EqualTo(updatedProduct.CreateDate));
            Assert.That(firstProduct.isActive, Is.EqualTo(firstProduct.isActive));
        }

        [Test]
        public void DeleteProductTest()
        {
            int maxID = products.Max(a => a.Id); // Before removal

            var productController = ControllerCreater("api/Products/" + maxID, HttpMethod.Delete);
            
            var lastProduct = products.Last();

            // Remove last Product
            Assert.That(productController.Delete(lastProduct.Id), Is.True);
            Assert.That(maxID, Is.GreaterThan(products.Max(a => a.Id))); // Max id reduced by 1
        }

        [Test]
        public void DeleteProductWithWrongIdTest()
        {
            var productController = ControllerCreater("api/Products/", HttpMethod.Delete);
            Assert.That(productController.Delete(-1), Is.False);
        }

        #endregion
    }
}
