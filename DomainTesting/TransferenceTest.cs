using System;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using Domain.Transaction;
using NUnit.Framework;

namespace DomainTesting
{
    public class TransferenceTest : Setup
    {

        public void InitializeTransferenceData()
        {
            ProviderTest.PopularProveedores();
            DocumentTest.PopularDocumentos();
            
            var localizacion = Localizacion.GetLocalizaciones()[0];
            var repuesto = Repuesto.GetRepuestos()[0];

            Existencia.AgregarExistencia(repuesto.Repuesto_ID, 10, localizacion.Codigo);
        }

        [Test]
        public void AgregarTransferencia()
        {
            this.InitializeTransferenceData();
            Assert.DoesNotThrow(() =>
            {
                var builder = new TransferenciaBuilder();
                builder.SetFecha(DateTime.Now);
                builder.SetBodegaDestino(1);
                builder.SetBodegaOrigen(1);
                builder.AgregarProductos(1, Localizacion.GetLocalizaciones()[1].Codigo, 5);
                builder.Build();
            });
        }

        [Test]
        public void TransferenciaParametrosErroneos()
        {
            
            this.InitializeTransferenceData();
            var builder = new TransferenciaBuilder();
            
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    builder.SetFecha(new DateTime(2016, 8, 8));
                }, "[ERROR]: Un ArgumentException deberia ser lanzado por asignar una fecha fuera del periodo activo.");

                Assert.Throws<InvalidOperationException>(
                    () => builder.SetBodegaDestino(4),
                    "[ERROR]: Una InvalidOperationException deberia ser lanzada por asignar al builder una bodega inexistente."
                );
            });
            
        }

        [Test]
        public void AgregarProductosParametrosErroneos()
        {
            
            this.InitializeTransferenceData();
            var builder = new TransferenciaBuilder();

            
            Assert.Multiple(() =>
            {
                
                Assert.Throws<InvalidOperationException>(() => builder.AgregarProductos(
                    1,
                    Localizacion.GetLocalizaciones()[1].Codigo,
                    10
                ), "[ERROR]: Un InvalidOperationException deberia ser lanzado al agregar una localizacion sin haber fijado una bodega de origen y destino.");
                
                builder.SetBodegaDestino(1);
                builder.SetBodegaOrigen(1);
                
                Assert.Throws<ArgumentException>(() => builder.AgregarProductos(
                4,
                Localizacion.GetLocalizaciones()[1].Codigo,
                10
                ), "[ERROR]: Un ArgumentException deberia ser lanzado al agregar una existencia no presente en la DB.");
                
                Assert.Throws<NullReferenceException>(() => builder.AgregarProductos(
                    1,
                    "1010",
                    10
                ), "[ERROR]: Un NullReferenceException deberia ser lanzado al agregar una localizacion erronea.");
                
                Assert.Throws<ArgumentOutOfRangeException>(() => builder.AgregarProductos(
                    1,
                    Localizacion.GetLocalizaciones()[1].Codigo,
                    0
                ), "[ERROR]: Un ArgumentException deberia ser lanzado al agregar un producto con unidades menores a 1.");
            });
            
        }

        [Test]
        public void ConstruirTransferencia()
        {
            
            this.InitializeTransferenceData();
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new TransferenciaBuilder();
                    builder.SetBodegaDestino(1);
                    builder.SetBodegaOrigen(1);
                    builder.Build();
                }, "[ERROR]: Un ArgumentNullException deberia ser lanzado cuando la fecha es nula.");
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new TransferenciaBuilder();
                    builder.SetBodegaOrigen(1);
                    builder.SetFecha(DateTime.Now);
                    builder.Build();
                }, "[ERROR]: Un ArgumentNullException deberia ser lanzado cuando la bodega de destino es nula.");
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var builder = new TransferenciaBuilder();
                    builder.SetFecha(DateTime.Now);
                    builder.SetBodegaOrigen(1);
                    builder.Build();
                }, "[ERROR]: Un ArgumentNullException deberia ser lanzado cuando la bodega de origen es nula.");
                
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var builder = new TransferenciaBuilder();
                    builder.SetFecha(DateTime.Now);
                    builder.SetBodegaOrigen(1);
                    builder.SetBodegaDestino(1);
                    builder.Build();
                }, "[ERROR]: Un InvalidOperationException deberia ser lanzado cuando se trata de construiir una transferencia sin productos.");
            });
            
        }
        
    }
}