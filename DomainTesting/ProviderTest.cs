using System;
using System.Data;
using Domain.Providers;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace DomainTesting
{
    public class ProviderTest : Setup
    {

        public void PopularProveedores()
        {
            Proveedor.AgregarProveedor("Proveedor #1");
        }

        [Test]
        public void CrearProveedores()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularProveedores();
                Proveedor.GetProveedor(1);
                Proveedor.GetProveedores();
            });
        }

        [Test]
        public void DuplicarProveedores()
        {
            this.PopularProveedores();
            Assert.Multiple(() =>
            {

                Assert.Throws<DuplicateNameException>(() =>
                {
                    Proveedor.AgregarProveedor("Proveedor #1");
                }, "[ERROR]: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Proveedores.");
                Console.WriteLine("[PRUEBA EXITOSA]: Se evito la creacion de un duplicado de Proveedores.");

            });
        }

        [Test]
        public void CrearProveedoresParametrosNulos()
        {
            this.PopularProveedores();
            Assert.Multiple(() => {
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Proveedor.AgregarProveedor(null);
                }, "[ERROR]: Una excepcion InvNullParameter deberia ser lanzada en Proveedor.");
                Console.WriteLine("[PRUEBA EXITOSA]: Se evito la creacion con parametros nulos en nombre de un Proveedor.");
                
            });
    }
        
    }
}