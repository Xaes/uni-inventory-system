using System.Data;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class StockTest : Setup
    {

        public void PopularExistencias()
        {
            
            ReplacementTest.PopularRepuestos();
            LocationTests.PopularLocalizaciones();

            var localizacion = Localizacion.GetLocalizaciones()[0];
            var repuesto = Repuesto.GetRepuestos()[0];

            Existencia.AgregarExistencia(repuesto.Repuesto_ID, 10, localizacion.Codigo);
            
        }

        [Test]
        public void CrearExistencias()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularExistencias();
                Existencia.GetExistencias();
                Existencia.FindExistencia(1);
            });
        }

        [Test]
        public void DuplicarExistencias()
        {
            this.PopularExistencias();
            Assert.Throws<DuplicateNameException>(() =>
            {
                this.PopularExistencias();
            }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Existencias.");
        }
        
    }
}