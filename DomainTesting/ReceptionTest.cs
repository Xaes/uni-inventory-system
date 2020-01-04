using System;
using Domain.Document;
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

        [Test]
        public void CrearRecepcion()
        {
            
            ProviderTest.PopularProveedores();
            DocumentTest.PopularDocumentos();

            var periodo = Periodo.AgregarPeriodo(2020, "Periodo 2020");
            periodo.Abrir();
            
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
    }
}