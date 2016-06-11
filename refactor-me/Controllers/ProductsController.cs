using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Services;
using System.Collections.Generic;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private IProductService _productService;
        private IProductOptionService _productOptionService;

        public ProductsController(IProductService productService, IProductOptionService productOptionService)
        {
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [Route]
        [HttpGet]
        public List<Product> GetAll()
        {
            return _productService.GetProducts();
        }

        [Route]
        [HttpGet]
        public List<Product> SearchByName(string name)
        {
            return _productService.GetProductsWithName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return product;
        }

        [Route]
        [HttpPost]
        public bool Create(Product product)
        {
            return _productService.SaveProduct(product);
        }

        [Route("{id}")]
        [HttpPut]
        public bool Update(Guid id, Product product)
        {
            return _productService.SaveProduct(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public bool Delete(Guid id)
        {
            return _productService.DeleteProduct(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public List<ProductOption> GetOptions(Guid productId)
        {
            return _productOptionService.GetProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var productOption = _productOptionService.GetProductOption(id);
            if (productOption == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return productOption;     
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public bool DeleteOption(Guid id)
        {
            return _productOptionService.DeleteProductOption(id);         
        }
    }
}
