using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Services;
using System.Collections.Generic;

namespace refactor_me.Controllers
{
    public class ProductController : ApiController
    {
        private IProductService _productService;
        private IProductOptionService _productOptionService;

        public ProductController(IProductService productService, IProductOptionService productOptionService)
        {
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [Route("api/Product/GetProducts")]
        [HttpGet]
        public List<Product> GetProducts()
        {
            return _productService.GetProducts();
        }

        [Route("api/Product/GetProduct/{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return product;
        }

        [Route("api/Product/GetProductByName/{name}")]
        [HttpGet]
        public List<Product> GetProductByName(string name)
        {
            return _productService.GetProductsWithName(name);
        }

        [Route("api/Product/SaveProduct")]
        [HttpPost]
        public bool SaveProduct([FromBody]Product product)
        {
            return _productService.SaveProduct(product);
        }

        [Route("api/Product/DeleteProduct/{id}")]
        [HttpDelete]
        public bool DeleteProduct(Guid id)
        {
            return _productService.DeleteProduct(id);
        }

    }
}
