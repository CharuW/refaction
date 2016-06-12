﻿using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Helpers
{
    public interface ISqlHelper
    {
        DataTable ExecuteReader(string sql);
        void ExecuteNonQuery(string sql);

    }

    public class SqlHelper : ISqlHelper
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        private SqlConnection _connection;

        public DataTable ExecuteReader(string sql)
        {
            if (_connection == null)
                _connection = GetConnection();

            _connection.Open();
            var cmd = new SqlCommand(sql, _connection);
            var reader = cmd.ExecuteReader();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            _connection.Close();

            return dataTable;
        }

        public void ExecuteNonQuery(string sql)
        {
            if (_connection == null)
                _connection = GetConnection();

            _connection.Open();
            var cmd = new SqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        private SqlConnection GetConnection()
        {
            var connectionInfo = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connectionInfo);
        }
    }
}