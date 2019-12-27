using System;
using Domain.Inventory;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace DomainTesting
{
    public class PeriodTest : Setup
    {

        public void PopularPeriodos()
        {
            Periodo.AgregarPeriodo(2018, "Año 2018");
            Periodo.AgregarPeriodo(2019, "Año 2019");
        }
        
        [Test]
        public void CrearPeriodos() 
        {
            Assert.DoesNotThrow(this.PopularPeriodos);
        }

        [Test]
        public void DuplicarPeriodos()
        {
            
            this.PopularPeriodos();
            Assert.Multiple(() =>
            {
                
                // Checkear Duplicidad por Periodo.
                
                Assert.Throws<SqlException>(() =>
                {
                    Periodo.AgregarPeriodo(2018, "Año 2018");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Periodo por periodo.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un Periodo.");
                
                // Checkear Duplicidad por Nombre.
                
                Assert.Throws<SqlException>(() =>
                {
                    Periodo.AgregarPeriodo(2020, "Año 2018");
                }, "ERROR: Una excepcion InvDuplicateException deberia ser lanzada en creacion de Periodo por nombre.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito la creacion de un duplicado de un Periodo.");
                
            });
            
        }

        [Test]
        public void CrearPeriodosParametrosNulos()
        {
            Assert.Throws<SqlException>(() =>
            {
                Periodo.AgregarPeriodo(2021, null);
            }, "ERROR: Una excepcion InvNullParameter deberia ser lanzada en creacion de Periodo por nombre.");
        }

        [Test]
        public void CambiarEstadoPeriodos()
        {
            
            this.PopularPeriodos();
            var periodo1 = Periodo.FindByPeriodoFiscal(2018);
            var periodo2 = Periodo.FindByPeriodoFiscal(2019);
            
            Assert.Multiple(() =>
            {
                
                Assert.DoesNotThrow(periodo1.Abrir);
                
                Assert.Throws<SqlException>(() =>
                {
                    periodo2.Abrir();
                }, "ERROR: Una excepcion InvInvalidOperation deberia ser lanzado por abrir un periodo habiendo otro ya abierto.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito abrir un Periodo habiendo otro ya abierto.");
                
                Assert.Throws<SqlException>(() =>
                {
                    periodo2.Cerrar();
                }, "ERROR: Una excepcion InvInvalidOperation deberia ser lanzado por cerrar un periodo sin haber estado activo.");
                Console.WriteLine("PRUEBA EXITOSA: Se evito cerrar un Periodo que no ha sido abierto.");
                
            });
            
        }
    }
}