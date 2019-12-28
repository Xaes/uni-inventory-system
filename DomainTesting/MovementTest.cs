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
                1000, 100, null, 10, DateTime.Now,
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
        public void CrearMovimientoParametrosNulos()
        {
            this.CrearMovimientos();
            Assert.Throws<ArgumentNullException>(() =>
            {
                Movimiento.AgregarMovimiento(null, null,
                    Periodo.FindPeriodo(2018).Periodo_ID, 1000, 100, null,
                    10, DateTime.Now, TipoTransaccion.SALIDA);
            });
        }
    }
}