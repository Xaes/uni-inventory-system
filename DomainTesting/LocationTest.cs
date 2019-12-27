using System;
using Domain.Locations;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace DomainTesting
{
    public class LocationTests : Setup
    {

        public void PopularLocalizaciones()
        {
            var bodega = Bodega.AgregarBodega("Bodega #1", "10000");
            var pasillo = bodega.AgregarPasillo("10001");
            var estante = pasillo.AgregarEstante("10002", 1);
            estante.AgregarLocalizacion();
        }

        [Test]
        public void CrearLocalizaciones()
        {
            Assert.DoesNotThrow(this.PopularLocalizaciones);
        }

        [Test]
        public void DuplicarLocalizaciones()
        {
            
            this.PopularLocalizaciones();
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];

            Assert.Multiple(() =>
            {
                
                // Checkear por Duplicidad en Codigo.
                
                Assert.Throws<SqlException>(() =>
                {
                    Bodega.AgregarBodega("Bodega #1", "10000");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Bodegas por codigo.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado por codigo de una Bodega.");
                
                // Checkear por Duplicidad en Nombre de Bodega.
                
                Assert.Throws<SqlException>(() =>
                {
                    Bodega.AgregarBodega("Bodega #1", "10001");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Bodegas por nombre.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado por nombre de una Bodega.");
                
                Assert.Throws<SqlException>(() =>
                {
                    bodega.AgregarPasillo("10001");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Estantes.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un Estante.");

                Assert.Throws<SqlException>(() =>
                {
                    pasillo.AgregarEstante("10002", 1);
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Pasillos.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un Pasillo.");

            });
            
        }
        
        [Test]
        public void CrearLocalizacionParametrosNulos()
        {
            
            this.PopularLocalizaciones();
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];
            
            Assert.Multiple(() =>
            {

                // Checkear por nulidad en Nombre.
                
                Assert.Catch(() => Bodega.AgregarBodega(null, "20000"),
                    "ERROR: Una excepcion InvNullParameter deberia ser lanzada en Bodega."
                );
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion con parametros nulos en nombre de una Bodega.");
                
                // Checkear por nulidad en Codigo.
                
                Assert.Catch(() => Bodega.AgregarBodega("Bodega #2", null),
                    "ERROR: Una excepcion InvNullParameter deberia ser lanzada en Bodega."
                );
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion con parametros nulos en codigo de una Bodega.");
                
                Assert.Catch(() => {
                        bodega.AgregarPasillo(null);
                    }, "ERROR: Una excepcion InvNullParameter deberia ser lanzada en Pasillo."
                );
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion con parametros nulos de un Pasillo.");
                
                Assert.Catch(() =>
                    {
                        pasillo.AgregarEstante(null, 1);
                    },
                    "ERROR: Una excepcion InvNullParameter deberia ser lanzada en Estante."
                );
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion con parametros nulos de un Estante.");
                
            });
        }
        
    }
}