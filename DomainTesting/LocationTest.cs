using System;
using Domain.Locations;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using NUnit.Framework;

namespace DomainTesting
{
    public class LocationTests : Setup
    {

        public void PopularLocalizaciones()
        {
            // Guardando Entidades.

            var bodega = Bodega.AgregarBodega("Bodega #1", "10000");
            var pasillo = bodega.AgregarPasillo("10001");
            var estante = pasillo.AgregarEstante("10002", 1);
            var localizacion = estante.AgregarLocalizacion();
            
            // Leyendo Entidades.
            
            Console.WriteLine(bodega.ToString());
            Console.WriteLine(pasillo.ToString());
            Console.WriteLine(estante.ToString());
            Console.WriteLine(localizacion.ToString());
        }

        [Test]
        public void CrearLocalizaciones()
        {
            this.PopularLocalizaciones();
            Assert.Pass();
        }

        [Test]
        public void DuplicarBodega()
        {
            this.PopularLocalizaciones();
            Assert.Throws<SqlException>(() =>
            {
                var bodega = Bodega.AgregarBodega("Bodega #1", "10000");
            });
        }
        
        [Test]
        public void DuplicarPasillo()
        {
            this.PopularLocalizaciones();
            Assert.Throws<SqlException>(() =>
            {
                var bodega = Bodega.FindBodega(1);
                var pasillo = bodega.AgregarPasillo("10001");
            });
        }
        
        [Test]
        public void DuplicarEstante()
        {
            this.PopularLocalizaciones();
            Assert.Throws<SqlException>(() =>
            {
                var bodega = Bodega.FindBodega(1);
                var pasillo = bodega.GetPasillos()[0];
                var estante = pasillo.AgregarEstante("10002", 1);
            });
        }
        
        [Test]
        public void DuplicarLocalizacion()
        {
            this.PopularLocalizaciones();
            Assert.Throws<SqlException>(() =>
            {
                var bodega = Bodega.FindBodega(1);
                var pasillo = bodega.GetPasillos()[0];
                var estante = pasillo.GetEstantes()[0];
                var localizacion = estante.AgregarLocalizacion();
            });
        }
        
    }
}