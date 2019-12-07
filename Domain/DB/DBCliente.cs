using System;
using Microsoft.Data.SqlClient;

namespace Domain.DB 
{
    public class DbCliente
    {

        private static readonly DbCliente Cliente = new DbCliente();
        private static SqlConnection Conexion;

        private DbCliente() { }

        public static void Init(string host, string dbName, string user, string password)
        {
            Console.WriteLine("--- Conectando a la DB ---");
            
            Conexion = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = host,
                    InitialCatalog = dbName,
                    UserID = user,
                    Password = password
                }.ConnectionString
            );
            
            Console.WriteLine("--- Conectado a HOST: {0} en DB: {1} con USUARIO: {2} --- ", host, dbName, user);
        }
        
        public static DbCliente GetClient() { return Cliente; }
        public static SqlConnection GetConexion() { return Conexion; }
    }
}