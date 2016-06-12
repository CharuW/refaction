using refactor_me.Helpers;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace refactor_me.Services
{
    public interface IProductService
    {
        Product GetProduct(Guid id);
        List<Product> GetProducts();
        List<Product> GetProductsWithName(string name);
        bool SaveProduct(Product product);
        bool DeleteProduct(Guid id);
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
            var table = _sqlHelper.ExecuteReader(sql);

            return GetProducts(table);
        }

        public List<Product> GetProductsWithName(string name)
        {
            var sql = $"select Id, Name, Description, Price, DeliveryPrice from product where lower(name) like '%{name.ToLower()}%'";
            var table = _sqlHelper.ExecuteReader(sql);

            return GetProducts(table);
        }

        public Product GetProduct(Guid id)
        {
            var sql = $"select * from product where id = '{id}'";
            var table = _sqlHelper.ExecuteReader(sql);

            var products = GetProducts(table);
            if (products.Count == 0) return null;
            return products[0];
        }

        private List<Product> GetProducts(DataTable table)
        {
            var products = new List<Product>();
            foreach (DataRow row in table.Rows)
            {
                var product = new Product()
                {
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

        public bool SaveProduct(Product product)
        {
            try
            {
                if (product.Id == Guid.Empty) product.Id = Guid.NewGuid();
                var sb = new StringBuilder();
                sb.AppendFormat("if not exists (select * from product where Id = '{0}')", product.Id);
                sb.AppendLine();
                sb.AppendLine("begin insert into product (Id, Name, Description, Price, DeliveryPrice)");
                sb.AppendFormat("values('{0}', '{1}', '{2}', {3}, {4}) end", product.Id, product.Name, 
                        product.Description, product.Price, product.DeliveryPrice);
                sb.AppendLine();
                sb.AppendFormat("else begin update product set Name = '{0}', Description = '{1}', ", 
                        product.Name, product.Description);
                sb.AppendFormat("Price = {0}, DeliveryPrice = {1} where Id = '{2}' end", 
                        product.Price, product.DeliveryPrice, product.Id);


                _sqlHelper.ExecuteNonQuery(sb.ToString());
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(Guid id)
        {
            try
            {
                var sql = $"delete from productoption where ProductId = '{id}'";
                _sqlHelper.ExecuteNonQuery(sql);
                sql = $"delete from product where Id = '{id}'";
                _sqlHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}