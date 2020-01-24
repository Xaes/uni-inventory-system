using System;
using Domain.DB;
using Domain.Document;
using NUnit.Framework;

namespace DomainTesting
{
    public class FilterTest
    {

        [Test]
        public void ConstruccionExpresion()
        {
            var builder = new FilterBuilder();
            
            builder.SetClase(typeof(LineaDocumento));
            builder.AgregarCriteria("Unidades", Criteria.Operador.IGUAL);
            builder.AgregarCriteria("UnidadesNoRecibidas", Criteria.Operador.DISTINTO);
            builder.SetOperador(FilterBuilder.Operador.O);
            
            Assert.AreEqual(
                "Select * From LineaDocumento Where Unidades = @Unidades or UnidadesNoRecibidas != @UnidadesNoRecibidas",
                builder.Build()
            );
        }

        [Test]
        public void ConstruccionParametrosErroneos()
        {
            var builder = new FilterBuilder();
            Assert.Multiple(() =>
            {
                Assert.Catch<InvalidOperationException>(() => builder.Build());

                Assert.Catch<ArgumentException>(() =>
                {
                    builder.SetClase(typeof(LineaDocumento));
                    builder.AgregarCriteria("AlgunCampoInexistente", Criteria.Operador.IGUAL);
                });
                
                Assert.Catch<InvalidOperationException>(() => { builder.Build(); });
            });
        }
        
    }
}