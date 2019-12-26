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
            this.PopularLocalizaciones();
            Assert.Pass("La creacion de Localizaciones tuvo un error.");
            Console.WriteLine("PRUEBA EXITOSA: Las Localizaciones se crearon correctamente.");
        }

        [Test]
        public void DuplicarLocalizaciones()
        {
            
            this.PopularLocalizaciones();
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];

            Assert.Multiple(() =>
            {
                
                Assert.Throws<SqlException>(() =>
                {
                    Bodega.AgregarBodega("Bodega #1", "10000");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Bodegas.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de una Bodega.");
                
                Assert.Throws<SqlException>(() =>
                {
                    bodega.AgregarPasillo("10001");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Estantes.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un Estante.");

                Assert.Throws<SqlException>(() =>
                {
                    pasillo.AgregarEstante("10002", 1);
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Pasillos.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un pasillo.");

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

                Assert.Catch(() => Bodega.AgregarBodega(null, null),
                    "ERROR: Una excepcion InvNullParameter deberia ser lanzada en Bodega."
                );
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion con parametros nulos de una Bodega.");
                
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