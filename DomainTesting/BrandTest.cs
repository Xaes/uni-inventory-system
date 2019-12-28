using System;
using System.Data;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class BrandTest : Setup
    {

        public static void PopularMarcas()
        {
            MarcaRepuesto.AgregarMarca("Toyota", false);
            MarcaRepuesto.AgregarMarca("Hyundai", true);
        }

        [Test]
        public void CrearMarcas()
        {
            Assert.DoesNotThrow(() =>
            { 
                PopularMarcas();
                MarcaRepuesto.FindMarca(1);
                MarcaRepuesto.GetMarcas();
            });
        }

        [Test]
        public void DuplicarMarcas()
        {
            PopularMarcas();
            Assert.Throws<DuplicateNameException>(
                () => MarcaRepuesto.AgregarMarca("Toyota", false),
                "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de MarcaRepuesto."
            );
        }

        [Test]
        public void CrearMarcasParametrosNulos()
        {
            Assert.Throws<ArgumentNullException>(
                () => MarcaRepuesto.AgregarMarca(null, false),
                "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de MarcaRepuesto."
            );
        }
        
    }
}