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

        [Route("api/ProductOptions/GetOptions")]
        [HttpGet]
        public List<ProductOption> GetOptions([FromUri]Guid productId)
        {
            return _productOptionService.GetProductOptions(productId);
        }

        [Route("api/ProductOptions/GetOption")]
        [HttpGet]
        public ProductOption GetOption([FromUri]Guid productId, [FromUri]Guid id)
        {
            var productOption = _productOptionService.GetProductOption(id);
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

        [Route("api/ProductOptions/DeleteOption")]
        [HttpDelete]
        public bool DeleteOption([FromUri]Guid id)
        {
            return _productOptionService.DeleteProductOption(id);         
        }
    }
}
