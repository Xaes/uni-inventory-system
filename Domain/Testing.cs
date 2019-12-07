using System;
using Domain.DB;
using Domain.Locations;

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
            
            // Trayendo todas las bodegas.
            
            Console.WriteLine(Sucursal.GetBodegas());
          
            // Trayendo entidades de Localizacion.
        
            Bodega bodega = Sucursal.FindBodega(1);
            Pasillo pasillo = bodega.GetPasillos()[0];
            Estante estante = pasillo.GetEstantes()[0];
            Localizacion localizacion = estante.GetLocalizaciones()[0];
            
            Console.WriteLine(bodega);
            Console.WriteLine(pasillo);
            Console.WriteLine(estante);
            Console.WriteLine(localizacion);
            
        }
    }
}

