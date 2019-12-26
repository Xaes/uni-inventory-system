using System;
using System.IO;
using System.Reflection;
using Dapper;
using Domain.DB;
using NUnit.Framework;

namespace DomainTesting
{
    public class Setup
    {
        
        private const string HOST = "localhost";
        private const string DBNAME = "inventorydb";
        private const string USER = "sa";
        private const string PASSWORD = "Password123!";
        
        [SetUp]
        public void TestSetup()
        {
            
            DbCliente.Init(HOST, DBNAME, USER, PASSWORD);
            
            // Borrando / Reescribiendo todas las tablas en DB.
            
            var initPath = AppDomain.CurrentDomain.BaseDirectory + @"../../../../SQL/InitDB.sql";
            var dropPath = AppDomain.CurrentDomain.BaseDirectory + @"../../../../SQL/DropDB.sql";
            var dropScript = File.ReadAllText(dropPath);
            var initScript = File.ReadAllText(initPath);
            
            // Ejecutando el SQL.

            Console.WriteLine("Limpiando DB...");
            DbCliente.GetConexion().Execute(dropScript);
            Console.WriteLine("Restaurando DB...");
            DbCliente.GetConexion().Execute(initScript);

        }
    }
}