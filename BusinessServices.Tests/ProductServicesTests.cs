using AutoMapper;
using BusinessEntities;
using DataModel.Models;
using DataModel.Repository;
using DataModel.UnitOfWork;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsHelper;

namespace BusinessServices.Tests
{
    public class ProductServicesTests
    {
        #region fields 
        private IProductServices productService;
        private IUnitOfWork unitOfWork;
        private List<Product> expectedProducts;
        private GenericRepository<Product> productRepository;
        private DataModelContext dataModelContext;
        #endregion

        #region Set tests environment
  
        [OneTimeSetUp]
        public void Setup()
        {
            
        }

        [SetUp]
        public void ReInitializeTest()
        {
            expectedProducts = SetUpProducts();
            dataModelContext = new Mock<DataModelContext>().Object;
            productRepository = SetUpProductRepository();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.SetupGet(s => s.ProductRepository).Returns(productRepository);
            unitOfWork = unitOfWorkMock.Object;
            productService = new ProductServices(unitOfWork);
        }        

        private GenericRepository<Product> SetUpProductRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<GenericRepository<Product>>(MockBehavior.Default, dataModelContext);

            // Setup mocking behavior
            mockRepo
                .Setup(p => p.GetAll())
                .Returns(expectedProducts);

            mockRepo
                .Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, Product>(id => expectedProducts.Find(p => p.Id.Equals(id))));

            mockRepo
                .Setup(p => p.Insert((It.IsAny<Product>())))
                .Callback(new Action<Product>(newProduct =>
                {
                    dynamic maxProductID = expectedProducts.Last().Id;
                    dynamic nextProductID = maxProductID + 1;
                    newProduct.Id = nextProductID;
                    expectedProducts.Add(newProduct);
                }));

            mockRepo
                .Setup(p => p.Update(It.IsAny<Product>()))
                .Callback(new Action<Product>(prod =>
                {
                    var oldProduct = expectedProducts.Find(a => a.Id == prod.Id);
                    oldProduct = prod;
                }));

            mockRepo
                .Setup(p => p.Delete(It.IsAny<Product>()))
                .Callback(new Action<Product>(prod =>
                {
                    var productToRemove = expectedProducts.Find(a => a.Id == prod.Id);

                    if (productToRemove != null)
                        expectedProducts.Remove(productToRemove);
                }));

            // Return mock implementation object
            return mockRepo.Object;
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
            expectedProducts = null;
        }

        [TearDown]
        public void DisposeTest()
        {
            productService = null;
            unitOfWork = null;
            productRepository = null;
            if (dataModelContext != null)
                dataModelContext.Dispose();
        }

        #endregion

        #region Tests 
        [Test]
        public async Task GetAllProductsTestForNull()
        {
            expectedProducts.Clear();
            var products = await productService.GetAllProducts();
            Assert.Null(products);
        }

        [Test]
        public async Task GetAllProductsTest()
        {
            var products = await productService.GetAllProducts();

            var productList = 
                products.Select(productEntity =>
                    new Product { Id = productEntity.Id, Name = productEntity.Name, CreateDate = productEntity.CreateDate, isActive = productEntity.isActive }).
                ToList(); 

            var comparer = new ProductComparer();

            CollectionAssert.AreEqual(
                productList.OrderBy(product => product, comparer),
                expectedProducts.OrderBy(product => product, comparer),
                comparer);
        }

        [Test]
        public void GetProductByRightIdTest()
        {
            var rigthId = 2;
            var coreServiceProduct = productService.GetProductById(rigthId);
            if (coreServiceProduct != null)
            {
                var productModel = new Product
                    { Id = coreServiceProduct.Id, Name = coreServiceProduct.Name, CreateDate = coreServiceProduct.CreateDate, isActive = coreServiceProduct.isActive };
                AssertObjects.PropertyValuesAreEquals(productModel,
                                                      expectedProducts.Find(a => (a.Id == rigthId)));
            }
        }

        [Test]
        public void GetProductByWrongIdTest()
        {
            var product = productService.GetProductById(0);
            Assert.Null(product);
        }

        [Test]
        public void AddNewProductTest()
        {
            var newProduct = new ProductEntity()
            {
                Name = "Web Api Service",
                CreateDate = DateTime.Now,
                isActive = false
            };

            var maxProductIDBeforeAdd = expectedProducts.Max(a => a.Id);
            newProduct.Id = maxProductIDBeforeAdd + 1;
            productService.CreateProduct(newProduct);
            var addedproduct = new Product
                { Id = newProduct.Id, Name = newProduct.Name, CreateDate = newProduct.CreateDate, isActive = newProduct.isActive };

            AssertObjects.PropertyValuesAreEquals(addedproduct, expectedProducts.Last());
            Assert.That(maxProductIDBeforeAdd + 1, Is.EqualTo(expectedProducts.Last().Id));
        }

        [Test]
        public void UpdateProductTest()
        {
            DateTime createDate = DateTime.Now.AddMinutes(-1);
            string name = "Web Api Server";
            bool isActive = false;

            var firstProduct = expectedProducts.First();

            var updatedProduct = Mapper.Map<Product, ProductEntity>(firstProduct);
            updatedProduct.Name = name;
            updatedProduct.CreateDate = createDate;
            updatedProduct.isActive = isActive;

            productService.UpdateProduct(firstProduct.Id, updatedProduct);

            Assert.That(firstProduct.Id, Is.EqualTo(1)); // hasn't changed
            Assert.That(firstProduct.Name, Is.EqualTo(name));
            Assert.That(firstProduct.CreateDate, Is.EqualTo(createDate));
            Assert.That(firstProduct.isActive, Is.EqualTo(isActive));
        }

        [Test]
        public void DeleteProductTest()
        {
            int maxID = expectedProducts.Max(a => a.Id); // Before removal
            var lastProduct = expectedProducts.Last();

            // Remove last Product
            productService.DeleteProduct(lastProduct.Id);
            Assert.That(maxID, Is.GreaterThan(expectedProducts.Max(a => a.Id)));
        }
        #endregion
    }
}
