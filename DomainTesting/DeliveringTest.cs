using System;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using Domain.Providers;
using Domain.Transaction;
using NUnit.Framework;

namespace DomainTesting
{
    public class DeliveringTest : Setup
    {
        public void InitializeDeliveringTest()
        {
            ProviderTest.PopularProveedores();
            DocumentTest.PopularDocumentos();
            
            var builder = new RecepcionBuilder();
            builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
            builder.SetFecha(DateTime.Now);
            builder.SetProveedor(Proveedor.FindProveedor(1).Proveedor_ID);
            builder.AgregarProductos(
                Repuesto.FindRepuesto(1).Repuesto_ID,
                10,
                1,
                1,
                5,
                100F,
                Localizacion.GetLocalizaciones()[0]
            );
            builder.AgregarProductos(
                Repuesto.FindRepuesto(1).Repuesto_ID,
                5,
                0,
                0,
                5,
                125F,
                Localizacion.GetLocalizaciones()[0]
            );
            builder.Build();
        }

        [Test]
        public void CrearEntrega()
        {
            this.InitializeDeliveringTest();
            Assert.DoesNotThrow(() =>
            {
                var builder = new EntregaBuilder();
                builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
                builder.SetFecha(DateTime.Now);
                builder.SetProveedor(Proveedor.FindProveedor(1).Proveedor_ID);
                builder.AgregarProductos(Existencia.FindExistencia(1).Existencia_ID, 5, 150F);
                builder.Build();
            });
            
        }
        
        [Test]
        public void AgregarProductosParametrosErroneos()
        {

            this.InitializeDeliveringTest();
            Assert.Multiple(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.AgregarProductos(Existencia.FindExistencia(1).Existencia_ID, 5, 150F);
                }, "[ERROR]: Una InvalidOperationException deberia ser lanzada en la creacion de un Producto sin haber agregado una Bodega primero.");
                
                Assert.Throws<ArgumentException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.SetBodega(1);
                    builder.AgregarProductos(5, 5, 150F);
                }, "[ERROR]: Un ArgumentException deberia ser lanzado al agregar una existencia invalida.");
                
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.SetBodega(1);
                    builder.AgregarProductos(Existencia.FindExistencia(1).Existencia_ID, -1, -1);
                }, "[ERROR]: Un ArgumentOutOfRangeException deberia ser lanzado al agregar una existencia con unidades negativas o 0.");

                Assert.Throws<ArgumentException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.SetBodega(1);
                    
                    // Agregando Nueva Localizacion.

                    Bodega.AgregarBodega("Bodega #2", "20000")
                        .AgregarPasillo("20001")
                        .AgregarEstante("20002", 1)
                        .AgregarLocalizacion();

                    Existencia.AgregarExistencia(1, 20, Localizacion.GetLocalizaciones()[2].Codigo);
                    builder.AgregarProductos(Existencia.FindExistencia(2).Existencia_ID, 5, 150F);
                }, "[ERROR]: Un ArgumentException deberia ser lanzando por agregar una existencia con localizacion fuera de la bodega establecida en el Documento.");

            });
        }

        [Test]
        public void ConstruirRecepcion()
        {
            this.InitializeDeliveringTest();
            Assert.Multiple(() =>
            {
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
                    builder.SetProveedor(Proveedor.FindProveedor(1).Proveedor_ID);
                    builder.AgregarProductos(Existencia.FindExistencia(1).Existencia_ID, 5, 150F);
                    builder.Build();
                }, "[ERROR]: Un ArgumentNullException deberia ser lanzando por pasar un parametro nulo.");

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new EntregaBuilder();
                    builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
                    builder.SetFecha(new DateTime(2020, 9, 28));
                    builder.SetProveedor(Proveedor.FindProveedor(1).Proveedor_ID);
                    builder.Build();
                }, "[ERROR]: Un ArgumentNullException deberia ser lanzando por pasar un parametro nulo.");
                
            });
        }
        
    }
}