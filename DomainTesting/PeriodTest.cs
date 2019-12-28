using System;
using System.Data;
using Domain.Inventory;
using NUnit.Framework;

namespace DomainTesting
{
    public class PeriodTest : Setup
    {

        public static void PopularPeriodos()
        {
            Periodo.AgregarPeriodo(2018, "Año 2018");
            Periodo.AgregarPeriodo(2019, "Año 2019");
        }
        
        [Test]
        public void CrearPeriodos() 
        {
            Assert.DoesNotThrow(() =>
            {
                PopularPeriodos();
                Periodo.GetPeriodos();
                Periodo.GetPeriodoActivo();
                Periodo.FindByPeriodoFiscal(1);
            });
        }

        [Test]
        public void DuplicarPeriodos()
        {
            
            PopularPeriodos();
            Assert.Multiple(() =>
            {
                
                // Checkear Duplicidad por Periodo.
                
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Periodo.AgregarPeriodo(2018, "Año 2018");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Periodo por periodo.");

                // Checkear Duplicidad por Nombre.
                
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Periodo.AgregarPeriodo(2020, "Año 2018");
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Periodo por nombre.");
                
            });
            
        }

        [Test]
        public void CrearPeriodosParametrosNulos()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Periodo.AgregarPeriodo(2021, null);
            }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de Periodo por nombre.");
        }

        [Test]
        public void CambiarEstadoPeriodos()
        {
            
            PopularPeriodos();
            var periodo1 = Periodo.FindByPeriodoFiscal(2018);
            var periodo2 = Periodo.FindByPeriodoFiscal(2019);
            
            Assert.Multiple(() =>
            {
                
                Assert.DoesNotThrow(periodo1.Abrir);
                
                Assert.Throws<InvalidOperationException>(() =>
                {
                    periodo2.Abrir();
                }, "[ERROR]: Una excepcion InvalidOperationException deberia ser lanzado por abrir un periodo habiendo otro ya abierto.");

                Assert.Throws<InvalidOperationException>(() =>
                {
                    periodo2.Cerrar();
                }, "[ERROR]: Una excepcion InvalidOperationException deberia ser lanzado por cerrar un periodo sin haber estado activo.");

            });
            
        }
    }
}