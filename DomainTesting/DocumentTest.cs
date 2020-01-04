using System;
using System.Data;
using Domain.Document;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class DocumentTest : Setup
    {

        public void PopularDocumentos()
        {
            MovementTest.PopularMovimientos();
            ReplacementTest.PopularRepuestos();

            var bodega = Bodega.FindBodega(1);
            var repuesto = Repuesto.FindRepuesto(1);
            
            var tipoDocumento = TipoDocumento.AgregarTipo(1, "Entrada por Compra", true, true);
            var documento = Documento.AgregarDocumento(null, tipoDocumento.TipoDocumento_ID, DateTime.Now);
            documento.AgregarLinea(Movimiento.FindMovimiento(1).Movimiento_ID,
                repuesto.Repuesto_ID, bodega.Bodega_ID, 10, 1, 1, 3,
                10.50F, null);
        }

        [Test]
        public void CrearDocumentos()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularDocumentos();
                TipoDocumento.FindTipoDocumento(1);
                TipoDocumento.GetTipoDocumentos();
                Documento.FindDocumento(1);
                Documento.GetDocumentos();
            });
        }

        [Test]
        public void DuplicarDocumentos()
        {
            
            this.PopularDocumentos();
            var tipoDocumento = TipoDocumento.FindTipoDocumento(1);
            
            Assert.Multiple(() =>
            {
                Assert.Throws<DuplicateNameException>(() => TipoDocumento.AgregarTipo(1, "Entrada por Compra", true, true),
                    "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en TipoDocumento."
                );
                
                Assert.Throws<DuplicateNameException>(() => Documento.AgregarDocumento(null, tipoDocumento.TipoDocumento_ID, DateTime.Now),
                    "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en TipoDocumento."
                );
                
            });
        }

        [Test]
        public void CrearDocumentosParametrosNulos()
        {
            this.PopularDocumentos();
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(
                    () => TipoDocumento.AgregarTipo(1, null, true, true),
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en TipoDocumento."
                );
            });
        }
        
    }
}