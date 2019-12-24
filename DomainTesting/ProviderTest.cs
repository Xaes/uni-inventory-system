using System;
using Domain.Providers;
using NUnit.Framework;

namespace DomainTesting
{
    public class ProviderTest : Setup
    {

        [Test]
        public void ProbarProveedores()
        {
            
            // Creando / Guardando un nuevo Proveedor.
            
            var proveedor = Proveedor.AgregarProveedor("Proveedor #1");
            Console.WriteLine(proveedor.ToString());
            
            Assert.Pass();
            
        }
        
    }
}