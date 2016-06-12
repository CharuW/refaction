using refactor_me.Helpers;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace refactor_me.Services
{
    public interface IProductOptionService
    {
        List<ProductOption> GetProductOptions(Guid productId);
        ProductOption GetProductOption(Guid id, Guid productId);
        bool DeleteProductOption(Guid id, Guid productId);
        bool SaveProductOption(ProductOption productOption);
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

        public ProductOption GetProductOption(Guid id, Guid productId)
        {
            var sql = $"select * from productoption where Id = '{id}' and ProductId = '{productId}'";
            var table = _sqlHelper.ExecuteReader(sql);

            var productOptions = GetProductOptions(table);
            if (productOptions.Count == 0) return null;
            return productOptions[0];
        }

        public bool SaveProductOption(ProductOption productOption)
        {
            try
            {
                if (productOption.Id == Guid.Empty) productOption.Id = Guid.NewGuid();
                var sb = new StringBuilder();
                sb.AppendFormat("if not exists (select * from productoption where Id = '{0}' and ProductId = '{1}')", 
                                productOption.Id, productOption.ProductId);
                sb.AppendLine();
                sb.AppendLine("begin insert into productoption (Id, ProductId, Name, Description)");
                sb.AppendFormat("values('{0}', '{1}', '{2}', '{3}') end", productOption.Id, productOption.ProductId, 
                                productOption.Name, productOption.Description);
                sb.AppendLine();
                sb.AppendFormat("else begin update productoption set Name = '{0}', Description = '{1}' ", 
                                productOption.Name, productOption.Description);
                sb.AppendFormat("where Id = '{0}' and ProductId = '{1}' end", productOption.Id, productOption.ProductId);


                _sqlHelper.ExecuteNonQuery(sb.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProductOption(Guid id, Guid productId)
        {
            try
            {
                var sql = $"delete from productoption where Id = '{id}' and ProductId = '{productId}'";
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