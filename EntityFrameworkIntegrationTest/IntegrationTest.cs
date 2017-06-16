using BusinessServices;
using DataModel.Models;
using DataModel.Repository;
using DataModel.UnitOfWork;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest
{
    public class IntegrationTest
    {
        protected DataModelContext DbContext;
        protected GenericRepository<Product> productRepository;
        protected IUnitOfWork unitOfWork;
        protected IProductServices productService;

        [OneTimeSetUp]
        public void Setup()
        {
            DbContext = new DataModelContext(new DropCreateDatabaseAlways<DataModelContext>());
            DbContext.Database.Create();

            GenericRepository<Product> productRepository = new GenericRepository<Product>(DbContext);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.SetupGet(s => s.ProductRepository).Returns(productRepository);
            unitOfWork = unitOfWorkMock.Object;
            productService = new ProductServices(unitOfWork);
        }

        [OneTimeTearDown]
        public void TestCleanup()
        {
            DbContext.Database.Delete();
        }

        //[Test]
        //public async void GetAllProductsTestForIsNotEmpty()
        //{
        //    var products = await productService.GetAllProducts();
        //    Assert.IsNotNull(products);
        //}

        //[Test]
        //public void UpdateProductTest()
        //{
        //    DateTime createDate = DateTime.Now.AddMinutes(-1);
        //    string name = "Web Api Server";
        //    bool isActive = false;

        //    productService.GetProductById(1);
        //    //var firstProduct = expectedProducts.First();

        //    //var updatedProduct = Mapper.Map<Product, ProductEntity>(firstProduct);
        //    //updatedProduct.Name = name;
        //    //updatedProduct.CreateDate = createDate;
        //    //updatedProduct.isActive = isActive;

        //    //productService.UpdateProduct(firstProduct.Id, updatedProduct);

        //    //Assert.That(firstProduct.Id, Is.EqualTo(1)); // hasn't changed
        //    //Assert.That(firstProduct.Name, Is.EqualTo(name));
        //    //Assert.That(firstProduct.CreateDate, Is.EqualTo(createDate));
        //    //Assert.That(firstProduct.isActive, Is.EqualTo(isActive));
        //}
    }
}
