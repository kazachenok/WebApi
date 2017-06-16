using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductService.Controllers
{
    public class ProductsController : ApiController
    {
        private IProductServices productServices;

        public ProductsController(IProductServices productServices)
        {
            this.productServices = productServices;
        }

        // GET: api/Product
        public async Task<HttpResponseMessage> Get()
        {
            var products = await productServices.GetAllProducts();
            if (products != null)
            {
                var productEntities = products as List<ProductEntity> ?? products.ToList();
                if (productEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, productEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found");
        }

        // GET: api/Product/5
        public HttpResponseMessage Get(int id)
        {
            var product = productServices.GetProductById(id);
            if (product != null)
                return Request.CreateResponse(HttpStatusCode.OK, product);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
        }

        // POST: api/Product
        public int Post([FromBody] ProductEntity productEntity)
        {
            return productServices.CreateProduct(productEntity);
        }


        // PUT api/product/5
        public bool Put(int id, [FromBody]ProductEntity productEntity)
        {
            if (id > 0)
            {
                return productServices.UpdateProduct(id, productEntity);
            }
            return false;
        }

        // DELETE api/product/5
        public bool Delete(int id)
        {
            if (id > 0)
                return productServices.DeleteProduct(id);
            return false;
        }
    }
}
