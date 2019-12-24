using System;
using Domain.DB;
using Domain.Locations;
using Domain.Providers;
using Domain.Inventory;

namespace Domain
{
    class Testing
    {
        
        private const string HOST = "localhost";
        private const string DBNAME = "inventorydb";
        private const string USER = "sa";
        private const string PASSWORD = "Password123!";
        
        public static void Main()
        {
            
            // Conectando a la DB.
            
            DbCliente.Init(HOST, DBNAME, USER, PASSWORD);
            
            // Guardando Entidades.

            var bodega = Bodega.AgregarBodega("Bodega #1", "10000");
            var pasillo = bodega.AgregarPasillo("10001");
            var estante = pasillo.AgregarEstante("10002", 1);
            var localizacion = estante.AgregarLocalizacion();
            
            // Leyendo Entidades.
            
            Console.WriteLine(bodega.ToString());
            Console.WriteLine(pasillo.ToString());
            Console.WriteLine(estante.ToString());
            Console.WriteLine(localizacion.ToString());

            // Creando / Guardando un nuevo Proveedor.
            
            var proveedor = Proveedor.AgregarProveedor("Proveedor #1");
            Console.WriteLine(proveedor.ToString());
            
            // Creando / Abriendo / Guardando un nuevos Periodos.

            var periodo1 = Periodo.AgregarPeriodo(2018, "Año 2018");
            var periodo2 = Periodo.AgregarPeriodo(2019, "Año 2019");
            periodo2.Abrir();
            
            Console.WriteLine("Periodo Activo: " + Periodo.GetPeriodoActivo().ToString());
            Console.WriteLine("Periodo Sin Usar: " + periodo1.ToString());
            Console.WriteLine(Periodo.GetPeriodos());
            
        }
    }
}

