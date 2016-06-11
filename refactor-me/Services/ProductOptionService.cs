using refactor_me.Helpers;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace refactor_me.Services
{
    public interface IProductOptionService
    {
        List<ProductOption> GetProductOptions(Guid productId);
        ProductOption GetProductOption(Guid id);
        bool DeleteProductOption(Guid id);
    }

    public class ProductOptionService : IProductOptionService
    {
        private ISqlHelper _sqlHelper;
        public ProductOptionService(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public List<ProductOption> GetProductOptions(Guid productId)
        {
            var sql = $"select Id, ProductId, Name, Description from productoption where ProductId = '{productId}'";
            var table = _sqlHelper.ExecuteReader(sql);
            return GetProductOptions(table);
        }

        private List<ProductOption> GetProductOptions(DataTable table)
        {
            var productOptions = new List<ProductOption>();
            foreach (DataRow row in table.Rows)
            {
                var productOption = new ProductOption()
                {
                    Id = Guid.Parse(row["Id"].ToString()),
                    ProductId = Guid.Parse(row["ProductId"].ToString()),
                    Name = row["Name"].ToString(),
                    Description = (DBNull.Value == row["Description"]) ? null : row["Description"].ToString()
                };

                productOptions.Add(productOption);
            }
            return productOptions;
        }

        public ProductOption GetProductOption(Guid id)
        {
            var sql = $"select * from productoption where Id = '{id}'";
            var table = _sqlHelper.ExecuteReader(sql);

            var productOptions = GetProductOptions(table);
            if (productOptions.Count == 0) return null;
            return productOptions[0];
        }

        public bool DeleteProductOption(Guid id)
        {
            try
            {
                var sql = $"delete from productoption where Id = '{id}'";
                _sqlHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}