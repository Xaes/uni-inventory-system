using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Domain.DB 
{
    public class Client
    {

        private static readonly Client DbClient = new Client();
        private static SqlConnection Connection;

        private Client() { }

        public void Init(string host, string dbName, string user, string password)
        {
            Console.WriteLine("--- Connecting to DB ---");
            
            Connection = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = host,
                    InitialCatalog = dbName,
                    UserID = user,
                    Password = password
                }.ConnectionString
            );
            
            Console.WriteLine("--- Connected to Host: {0} in DB: {1} with USER: {2} --- ", host, dbName, user);
        }
        
        public static Client GetClient() { return DbClient; }
        public static SqlConnection GetConnection() { return Connection; }
    }
}