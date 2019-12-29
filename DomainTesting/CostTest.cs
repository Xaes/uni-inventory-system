using System;
using Domain.Document;
using Domain.Inventory;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class CostTest : Setup
    {

        public void PopularCostos()
        {
            ReplacementTest.PopularRepuestos();
            var repuesto = Repuesto.FindRepuesto(1);

            Costo.AgregarCosto(repuesto.Repuesto_ID, 10, DateTime.Now, 10.50F);
        }

        [Test]
        public void CrearCostos()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularCostos();
                Costo.FindCosto(1);
                Costo.GetCostos();
            });
        }

        [Test]
        public void CrearCostosLlaveForaneaErronea()
        {
            Assert.Throws<ArgumentException>(
                () => Costo.AgregarCosto(0, 10, DateTime.Now, 10F),
                "[ERROR]: Una excepcion ArgumentException deberia ser lanzada en creacion de Costos cuando el codigo de Repuesto es erroneo."
            );
        }
        
    }
}