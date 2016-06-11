using refactor_me.Helpers;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace refactor_me.Services
{
    public interface IProductService
    {
        Product GetProduct(Guid id);
        List<Product> GetProducts();
        List<Product> GetProductsWithName(string name);
    }

    public class ProductService : IProductService
    {
        private ISqlHelper _sqlHelper;
        public ProductService(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            var sql = "select Id, Name, Description, Price, DeliveryPrice from product";
            var table = _sqlHelper.ExecuteCommand(sql);

            return CreateProducts(table);
        }

        public List<Product> GetProductsWithName(string name)
        {
            var sql = $"select Id, Name, Description, Price, DeliveryPrice from product where lower(name) like '%{name.ToLower()}%'";
            var table = _sqlHelper.ExecuteCommand(sql);

            return CreateProducts(table);
        }

        public Product GetProduct(Guid id)
        {
            throw new NotImplementedException();
        }


        private List<Product> CreateProducts(DataTable table)
        {
            var products = new List<Product>();
            foreach (DataRow row in table.Rows)
            {
                var product = new Product()
                {
                    IsNew = false,
                    Id = Guid.Parse(row["Id"].ToString()),
                    Name = row["Name"].ToString(),
                    Description = (DBNull.Value == row["Description"]) ? null : row["Description"].ToString(),
                    Price = decimal.Parse(row["Price"].ToString()),
                    DeliveryPrice = decimal.Parse(row["DeliveryPrice"].ToString())
                };

                products.Add(product);
            }
            return products;
        }
    
    }
}