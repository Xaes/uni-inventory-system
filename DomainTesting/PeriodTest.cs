using System;
using Domain.Inventory;
using NUnit.Framework;

namespace DomainTesting
{
    public class PeriodTest : Setup
    {
        [Test]
        public void CrearPeriodos() 
        {
            
            // Creando / Abriendo / Guardando un nuevos Periodos.

            var periodo1 = Periodo.AgregarPeriodo(2018, "Año 2018");
            var periodo2 = Periodo.AgregarPeriodo(2019, "Año 2019");
            periodo2.Abrir();
            
            Console.WriteLine("Periodo Activo: " + Periodo.GetPeriodoActivo());
            Console.WriteLine("Periodo Sin Usar: " + periodo1);

            Assert.Pass();
            
        }
    }
}