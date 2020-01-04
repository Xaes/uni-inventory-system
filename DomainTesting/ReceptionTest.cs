using System;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using Domain.Providers;
using Domain.Transaction;
using NUnit.Framework;

namespace DomainTesting
{
    public class ReceptionTest : Setup
    {

        public void InitializeReceptionData()
        {
            ProviderTest.PopularProveedores();
            DocumentTest.PopularDocumentos();
            Periodo.AgregarPeriodo(2020, "Periodo 2020").Abrir();
        }

        [Test]
        public void CrearRecepcion()
        {
            
            this.InitializeReceptionData();
            Assert.DoesNotThrow(() =>
            {
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
                builder.Build();
            });
            
        }

        [Test]
        public void RecepcionParametrosErroneos()
        {
            
            this.InitializeReceptionData();
            Assert.Multiple(() => {

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(5);
                }, "[ERROR]: Un InvalidOperationException: deberia ser lanzada al pasar una Bodega Inexistente.");

                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetProveedor(5);
                }, "[ERROR]: Un InvalidOperationException: deberia ser lanzada al pasar un Proveedor Inexistente.");

                Assert.Throws<ArgumentException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetFecha(new DateTime(2019, 8, 31));
                }, "[ERROR]: Un ArgumentException deberia ser lanzada al pasar una fecha fuera del periodo activo.");
                
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    Periodo.FindByPeriodoFiscal(2020).Cerrar();
                    builder.SetFecha(new DateTime(2019, 8, 31));
                }, "[ERROR]: Un InvalidOperationException deberia ser lanzada al pasar una fecha sin tener un periodo activo primero.");

            });
            
        }

        [Test]
        public void AgregarProductosParametrosErroneos()
        {
            this.InitializeReceptionData();
            Assert.Multiple(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.AgregarProductos(
                        Repuesto.FindRepuesto(1).Repuesto_ID,
                        10,
                        1,
                        1,
                        5,
                        100F,
                        Localizacion.GetLocalizaciones()[0]
                    );
                }, "[ERROR]: Una InvalidOperationException deberia ser lanzada en la creacion de un Producto sin haber agregado una Bodega primero.");
                
                Assert.Throws<ArgumentException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(1);
                    builder.AgregarProductos(
                        5,
                        10,
                        1,
                        1,
                        5,
                        100F,
                        Localizacion.GetLocalizaciones()[0]
                    );
                }, "[ERROR]: Un ArgumentException deberia ser lanzado al agregar un Producto Inexistente.");
                
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(1);
                    builder.AgregarProductos(
                        Repuesto.FindRepuesto(1).Repuesto_ID,
                        0,
                        -1,
                        -1,
                        0,
                        -1,
                        Localizacion.GetLocalizaciones()[0]
                    );
                }, "[ERROR]: Un ArgumentOutOfRangeException deberia ser lanzado al agregar un Producto con unidades negativas o 0.");

                Assert.Throws<ArgumentException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(1);
                    
                    // Agregando Nueva Localizacion.

                    Bodega.AgregarBodega("Bodega #2", "20000")
                        .AgregarPasillo("20001")
                        .AgregarEstante("20002", 1)
                        .AgregarLocalizacion();
                    
                    builder.AgregarProductos(
                        Repuesto.FindRepuesto(1).Repuesto_ID,
                        100,
                        10,
                        5,
                        1,
                        1000F,
                        Localizacion.GetLocalizaciones()[2]
                    );
                });

            });
        }

        [Test]
        public void ConstruirReception()
        {
            this.InitializeReceptionData();
            Assert.Multiple(() =>
            {
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
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
                    builder.Build();
                });
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
                    builder.SetFecha(new DateTime(2020, 9, 28));
                    builder.AgregarProductos(
                        Repuesto.FindRepuesto(1).Repuesto_ID,
                        10,
                        1,
                        1,
                        5,
                        100F,
                        Localizacion.GetLocalizaciones()[0]
                    );
                    builder.Build();
                });
                
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new RecepcionBuilder();
                    builder.SetBodega(Bodega.FindBodega(1).Bodega_ID);
                    builder.SetFecha(new DateTime(2020, 9, 28));
                    builder.SetProveedor(Proveedor.FindProveedor(1).Proveedor_ID);
                    builder.Build();
                });
                
            });
        }
    }
}