using System;
using Domain.Locations;
using NUnit.Framework;

namespace DomainTesting
{
    public class LocationTests : Setup
    {
        
        [Test]
        public void InitLocalizaciones()
        {
            
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

            Assert.Pass();
            
        }
        
    }
}