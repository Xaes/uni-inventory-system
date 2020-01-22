using System;
using System.Collections.Generic;
using System.Linq;
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
            Costo.AgregarCosto(repuesto.Repuesto_ID, 15, new DateTime(2019, 8, 30), 11F);
            Costo.AgregarCosto(repuesto.Repuesto_ID, 5, new DateTime(2019, 9, 29), 12F);
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
        public void ProyectarExtraccion()
        {
            this.PopularCostos();
            Assert.Multiple(() =>
            {
                
                var costos = Costo.ProyectarExtraccion(1, 29);
                Assert.AreEqual(3, costos.Count);
                Assert.AreEqual(15, costos[0]["cantidad"]);
                Assert.AreEqual(5, costos[1]["cantidad"]);
                Assert.AreEqual(9, costos[2]["cantidad"]);
                
                costos = Costo.ProyectarExtraccion(1, 30);
                Assert.AreEqual(3, costos.Count);
                Assert.AreEqual(15, costos[0]["cantidad"]);
                Assert.AreEqual(5, costos[1]["cantidad"]);
                Assert.AreEqual(10, costos[2]["cantidad"]);
                
                costos = Costo.ProyectarExtraccion(1, 8);
                Assert.AreEqual(1, costos.Count);
                Assert.AreEqual(8, costos[0]["cantidad"]);

                Assert.Throws<ArgumentOutOfRangeException>(() => Costo.ProyectarExtraccion(1, 0));
                Assert.Throws<InvalidOperationException>(() => Costo.ProyectarExtraccion(2, 1));
                
            });
        }

        [Test]
        public void ExtraerCostos()
        {
            PopularCostos();
            Assert.Multiple(() =>
            {
                Costo.ExtraerCosto(1, 8);
                Assert.AreEqual(3, Costo.GetCostos().Count);
                Assert.AreEqual(7, Costo.FindCosto(2).Unidades);
                Costo.ExtraerCosto(1, 8);
                Assert.AreEqual(2, Costo.GetCostos().Count);
                Assert.AreEqual(4, Costo.FindCosto(3).Unidades);
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

        [Test]
        public void CrearCostosParametrosErroneos()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Costo.AgregarCosto(1, 0, DateTime.Now, -1),
                "[ERROR]: Una excepcion ArgumentOutOfRangeException deberia ser lanzada en creacion de Costos cuando las unidades son 0 o negativas."
            );
        }
        
    }
}