using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Services;
using System.Collections.Generic;

namespace refactor_me.Controllers
{
    public class ProductsOptionsController : ApiController
    {
        private IProductOptionService _productOptionService;

        public ProductsOptionsController(IProductOptionService productOptionService)
        {
            _productOptionService = productOptionService;
        }

        [Route("api/ProductOptions/GetOptions/{productId}")]
        [HttpGet]
        public List<ProductOption> GetOptions(Guid productId)
        {
            return _productOptionService.GetProductOptions(productId);
        }

        [Route("api/ProductOptions/GetOption/{id}/{productId}")]
        [HttpGet]
        public ProductOption GetOption(Guid id, Guid productId)
        {
            var productOption = _productOptionService.GetProductOption(id, productId);
            if (productOption == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return productOption;     
        }

        [Route("api/ProductOptions/SaveOption")]
        [HttpPost]
        public bool SaveOption([FromBody]ProductOption option)
        {
            return _productOptionService.SaveProductOption(option);
        }

        [Route("api/ProductOptions/DeleteOption/{id}/{productId}")]
        [HttpDelete]
        public bool DeleteOption(Guid id, Guid productId)
        {
            return _productOptionService.DeleteProductOption(id, productId);         
        }
    }
}
