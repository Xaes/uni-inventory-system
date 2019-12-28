using System;
using System.Data;
using Domain.Document;
using Domain.Inventory;
using NUnit.Framework;

namespace DomainTesting
{
    public class DocumentTest : Setup
    {

        public void PopularDocumentos()
        {
            MovementTest.PopularMovimientos();
            var tipoDocumento = TipoDocumento.AgregarTipo(1, "Entrada por Compra", true, true);
            var documento = Documento.AgregarDocumento(null, tipoDocumento.TipoDocumento_ID, 10, DateTime.Now);
            //var linea = LineaDocumento.AgregarLinea(documento.Documento_ID, Movimiento.GetMovimiento(1))
        }

        [Test]
        public void CrearDocumentos()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularDocumentos();
                TipoDocumento.GetTipoDocumento(1);
                TipoDocumento.GetTipoDocumentos();
                Documento.GetDocumento(1);
                Documento.GetDocumentos();
            });
        }

        [Test]
        public void DuplicarDocumentos()
        {
            
            this.PopularDocumentos();
            var tipoDocumento = TipoDocumento.GetTipoDocumento(1);
            
            Assert.Multiple(() =>
            {
                Assert.Throws<DuplicateNameException>(() => TipoDocumento.AgregarTipo(1, "Entrada por Compra", true, true),
                    "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en TipoDocumento."
                );
                
                Assert.Throws<DuplicateNameException>(() => Documento.AgregarDocumento(null, tipoDocumento.TipoDocumento_ID, 10, DateTime.Now),
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