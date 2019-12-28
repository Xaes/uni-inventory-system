using System;
using System.Data;
using Domain.Locations;
using NUnit.Framework;

namespace DomainTesting
{
    public class LocationTests : Setup
    {

        public static void PopularLocalizaciones()
        {
            var bodega = Bodega.AgregarBodega("Bodega #1", "10000");
            var pasillo = bodega.AgregarPasillo("10001");
            var estante = pasillo.AgregarEstante("10002", 1);
            estante.AgregarLocalizacion();
        }

        [Test]
        public void CrearLocalizaciones()
        {
            Assert.DoesNotThrow(() =>
            {
                PopularLocalizaciones();
                var bodega = Bodega.FindBodega(1);
                var pasillo = bodega.GetPasillos()[0];
                var estante = pasillo.GetEstantes()[0];
                var localizacion = estante.GetLocalizaciones()[0];
            });
        }

        [Test]
        public void DuplicarLocalizaciones()
        {
            
            PopularLocalizaciones();
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];

            Assert.Multiple(() =>
            {
                
                // Checkear por Duplicidad en Codigo.
                
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Bodega.AgregarBodega("Bodega #1", "10000");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Bodegas por codigo.");

                // Checkear por Duplicidad en Nombre de Bodega.
                
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Bodega.AgregarBodega("Bodega #1", "10001");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Bodegas por nombre.");

                Assert.Throws<DuplicateNameException>(() =>
                {
                    bodega.AgregarPasillo("10001");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Estantes.");

                Assert.Throws<DuplicateNameException>(() =>
                {
                    pasillo.AgregarEstante("10002", 1);
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Pasillos.");

            });
            
        }
        
        [Test]
        public void CrearLocalizacionParametrosNulos()
        {
            
            PopularLocalizaciones();
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];

            Assert.Multiple(() =>
            {

                // Checkear por nulidad en Nombre.
                
                Assert.Throws<ArgumentNullException>(() => Bodega.AgregarBodega(null, "20000"),
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en Bodega."
                );

                // Checkear por nulidad en Codigo.
                
                Assert.Throws<ArgumentNullException>(() => Bodega.AgregarBodega("Bodega #2", null),
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en Bodega."
                );

                Assert.Throws<ArgumentNullException>(() => {
                        bodega.AgregarPasillo(null);
                    }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en Pasillo."
                );

                Assert.Throws<ArgumentNullException>(() =>
                    {
                        pasillo.AgregarEstante(null, 1);
                    },
                    "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en Estante."
                );

            });
        }
        
    }
}