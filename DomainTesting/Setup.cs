using System;
using System.IO;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace DomainTesting
{
    public class Setup
    {

        [SetUp]
        public void TestSetup()
        {
            
            DbCliente.Init(
                Environment.GetEnvironmentVariable("host"),
                Environment.GetEnvironmentVariable("dbName"),
                Environment.GetEnvironmentVariable("user"),
                Environment.GetEnvironmentVariable("password")
            );
            
            // Borrando / Reescribiendo todas las tablas en DB.
            
            var initPath = AppDomain.CurrentDomain.BaseDirectory + @"../../../../SQL/InitDB.sql";
            var dropPath = AppDomain.CurrentDomain.BaseDirectory + @"../../../../SQL/DropDB.sql";
            var dropScript = File.ReadAllText(dropPath);
            var initScript = File.ReadAllText(initPath);
            
            // Ejecutando el SQL.

            try
            {
                Console.WriteLine("[SETUP]: Limpiando DB...");
                DbCliente.GetConexion().Execute(dropScript);
            }
            catch (SqlException ex) when (ex.Number == 3701)
            {
                Console.WriteLine("[SETUP]: La DB ya estaba limpia.");
            }
            finally
            {
                Console.WriteLine("[SETUP]: Restaurando DB...");
                DbCliente.GetConexion().Execute(initScript);
            }
            
        }
    }
}