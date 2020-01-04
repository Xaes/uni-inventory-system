using System;
using System.Data;
using Domain.Providers;
using NUnit.Framework;

namespace DomainTesting
{
    public class ProviderTest : Setup
    {

        public static void PopularProveedores()
        {
            Proveedor.AgregarProveedor("Proveedor #1");
        }

        [Test]
        public void CrearProveedores()
        {
            Assert.DoesNotThrow(() =>
            {
                PopularProveedores();
                Proveedor.FindProveedor(1);
                Proveedor.GetProveedores();
            });
        }

        [Test]
        public void DuplicarProveedores()
        {
            PopularProveedores();
            Assert.Multiple(() =>
            {

                Assert.Throws<DuplicateNameException>(() =>
                {
                    Proveedor.AgregarProveedor("Proveedor #1");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Proveedores.");

            });
        }

        [Test]
        public void CrearProveedoresParametrosNulos()
        {
            PopularProveedores();
            Assert.Multiple(() => {
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Proveedor.AgregarProveedor(null);
                }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en Proveedor.");

            });
    }
        
    }
}