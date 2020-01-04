using System;
using Domain.Inventory;
using Domain.Locations;
using NUnit.Framework;

namespace DomainTesting
{
    public class MovementTest : Setup
    {

        public static void PopularMovimientos()
        {
            
            // Populando
            
            LocationTests.PopularLocalizaciones();
            PeriodTest.PopularPeriodos();

            // Trayendo Valores
            
            var bodega = Bodega.FindBodega(1);
            var pasillo = bodega.GetPasillos()[0];
            var estante = pasillo.GetEstantes()[0];
            estante.AgregarLocalizacion();
            var loc1 = estante.GetLocalizaciones()[0];
            var loc2 = estante.GetLocalizaciones()[1];
            var periodo = Periodo.FindPeriodo(2018);

            Movimiento.AgregarMovimiento(loc1.Codigo, loc2.Codigo, periodo.Periodo_ID,
                1000, 100, 10, DateTime.Now,
                TipoTransaccion.SALIDA);

        }

        [Test]
        public void CrearMovimientos()
        {
            Assert.DoesNotThrow(() =>
            {
                PopularMovimientos();
                Movimiento.FindMovimiento(1);
                Movimiento.GetMovimientos(); 
            });
        }
        
        [Test]
        public void CrearMovimientoMismaLocalizacion()
        {
            LocationTests.PopularLocalizaciones();
            PeriodTest.PopularPeriodos();
            var periodo = Periodo.FindPeriodo(2018);
            var loc1 = Localizacion.GetLocalizaciones()[0];

            Assert.Throws<ArgumentException>(() =>
            {
                Movimiento.AgregarMovimiento(loc1.Codigo, loc1.Codigo, periodo.Periodo_ID,
                    1000, 100, 10, DateTime.Now,
                    TipoTransaccion.SALIDA);
            }, "[ERROR]: Una excepcion ArgumentException deberia ser lanzada en creacion de Movimientos que tienen la misma localizacion como entrada y salida.");
        }

        [Test]
        public void CrearMovimientoParametrosNulos()
        {
            this.CrearMovimientos();
            var loc1 = Localizacion.GetLocalizaciones()[0];
            var loc2 = Localizacion.GetLocalizaciones()[1];
            var periodo = Periodo.FindPeriodo(2018);
            Assert.Multiple(() =>
            {
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Movimiento.AgregarMovimiento(null, null,
                        Periodo.FindPeriodo(2018).Periodo_ID, 1000, 100,
                        10, DateTime.Now, TipoTransaccion.SALIDA);
                }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de Movimientos cuando los codigo de localizacion son nulos.");
                
                Assert.DoesNotThrow(() =>
                {
                    Movimiento.AgregarMovimiento(loc1.Codigo, loc2.Codigo, periodo.Periodo_ID,
                        null, null, 10, DateTime.Now, TipoTransaccion.ENTRADA);
                });
            });
            
        }

        [Test]
        public void CrearMovimientoLlaveForeanaErronea()
        {
            LocationTests.PopularLocalizaciones();
            PeriodTest.PopularPeriodos();
            var periodo = Periodo.FindPeriodo(2018);
            var loc1 = Localizacion.GetLocalizaciones()[0];
            
            Assert.Throws<ArgumentException>(() =>
            {
                Movimiento.AgregarMovimiento("2-10002-10001-10000", loc1.Codigo, periodo.Periodo_ID,
                    1000, 100, 10, DateTime.Now,
                    TipoTransaccion.SALIDA);
            }, "[ERROR]: Una excepcion ArgumentException deberia ser lanzada en creacion de Movimientos cuando los codigo de localizacion son erroneos.");
        }
        
    }
}