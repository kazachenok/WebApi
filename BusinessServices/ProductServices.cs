using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using DataModel.UnitOfWork;
using DataModel.Models;
using BusinessEntities;
using AutoMapper;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class ProductServices : IProductServices
    {
        private IUnitOfWork unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductEntity>();
                cfg.CreateMap<Metadata, MetadataEntity>();
            });

            this.unitOfWork = unitOfWork;
        }

        public ProductEntity GetProductById(int productId)
        {
            var product = unitOfWork.ProductRepository.GetByID(productId);
            if (product != null)
            {
                var productModel = Mapper.Map<Product, ProductEntity>(product);
                return productModel;
            }
            return null; ;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProducts()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            if (products.Any())
            {
                var productsModel = Mapper.Map<List<Product>, List<ProductEntity>>(products);
                return productsModel;
            }
            return null;
        }

        public int CreateProduct(ProductEntity productEntity)
        {
            using (var scope = new TransactionScope())
            {
                var product = new Product
                {
                    Name = productEntity.Name,
                    CreateDate = productEntity.CreateDate == DateTime.MinValue ? (DateTime?)null : productEntity.CreateDate,
                    isActive = productEntity.isActive
                };

                unitOfWork.ProductRepository.Insert(product);
                unitOfWork.Save();
                scope.Complete();
                return product.Id;
            }
        }

        public bool UpdateProduct(int productId, BusinessEntities.ProductEntity productEntity)
        {
            var success = false;
            if (productEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var product = unitOfWork.ProductRepository.GetByID(productId);
                    if (product != null)
                    {
                        product.Name = productEntity.Name;
                        product.CreateDate = productEntity.CreateDate;
                        product.isActive = productEntity.isActive;

                        unitOfWork.ProductRepository.Update(product);
                        unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteProduct(int productId)
        {
            var success = false;
            if (productId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var product = unitOfWork.ProductRepository.GetByID(productId);
                    if (product != null)
                    {
                        unitOfWork.ProductRepository.Delete(product);
                        unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
