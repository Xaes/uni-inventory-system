using System;
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

        [Test]
        public void CrearExistenciasParametrosErroneos()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Existencia.AgregarExistencia(1, 0, Localizacion.GetLocalizaciones()[0].Codigo);
            }, "[ERROR]: Una excepcion ArgumentOutOfRangeException deberia ser lanzada en creacion de Existencia cuando las unidades son 0 o negativas.");
        }

        [Test]
        public void AgregarExistencias()
        {
            
            this.PopularExistencias();
            var existencia = Existencia.FindExistencia(1);
            
            Assert.Multiple(() =>
            {
                
                existencia.AgregarUnidades(5);
                
                // Checkear valor en Memoria.
                
                Assert.AreEqual(15, existencia.Unidades);
                
                // Checkear valor en la Base de Datos.

                existencia = Existencia.FindExistencia(1);
                Assert.AreEqual(15, existencia.Unidades);
                
                // Checkear lanzamiento de Exception.

                Assert.Throws<ArgumentOutOfRangeException>(() => existencia.AgregarUnidades(-1));

            });
            
        }

        [Test]
        public void ExtraerExistencias()
        {
            
            this.PopularExistencias();
            var existencia = Existencia.FindExistencia(1);
            
            Assert.Multiple(() =>
            {
                
                existencia.ExtraerUnidades(5);
                
                // Checkear valor en Memoria.
                
                Assert.AreEqual(5, existencia.Unidades);
                
                // Checkear valor en Base de Datos.
                
                existencia = Existencia.FindExistencia(1);
                Assert.AreEqual(5, existencia.Unidades);
                
                // Checkear lanzamiento de Exception.

                Assert.Throws<InvalidOperationException>(() => existencia.ExtraerUnidades(10));
                
                // Extrayendo todas las Unidades.

                existencia.ExtraerUnidades(5);
                Assert.AreEqual(0, existencia.Unidades);
                
                // La entidad en la Base de Datos deberia ser borrada.

                existencia = Existencia.FindExistencia(1);
                Assert.IsNull(existencia);

            });
            
        }
    }
}