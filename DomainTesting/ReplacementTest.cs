using System;
using System.Data;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class ReplacementTest : Setup
    {

        public static void PopularRepuestos()
        {
            CategoriesTest.PopulateCategorias();
            BrandTest.PopularMarcas();
            var marca = MarcaRepuesto.GetMarca(1);
            var subcat = Categoria.GetCategoria(1).GetSubCategorias()[0];
            Repuesto.AgregarRepuesto(
                "100-HF-120E",
                "Michelin Pilot Sport 4 SUV", 
                "La llanta ultra high performance para vehículos tipo SUV y CrossOver.",
                marca.MarcaRepuesto_ID,
                subcat.SubCategoria_ID,
                10000,
                4,
                1,
                "Unidades"
            );
        }

        [Test]
        public void AgregarRepuestos()
        {
            Assert.DoesNotThrow(() =>
            {
                PopularRepuestos();
                Repuesto.GetRepuesto(1);
                Repuesto.GetRepuestos();
            });
        }

        [Test]
        public void DuplicarRepuestos()
        {
            
            PopularRepuestos();
            var marca = MarcaRepuesto.GetMarca(1);
            var subcat = Categoria.GetCategoria(1).GetSubCategorias()[0];
            
            Assert.Multiple(() =>
            {
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Repuesto.AgregarRepuesto(
                        "100-HF-120E",
                        "Michelin Pilot Sport 4 SUV", 
                        "La llanta ultra high performance para vehículos tipo SUV y CrossOver.",
                        marca.MarcaRepuesto_ID,
                        subcat.SubCategoria_ID,
                        10000,
                        4,
                        1,
                        "Unidades"
                    );
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Repuesto por Codigo.");
                
                Assert.Throws<DuplicateNameException>(() =>
                {
                    Repuesto.AgregarRepuesto(
                        "100-HF-120T",
                        "Michelin Pilot Sport 4 SUV", 
                        "La llanta ultra high performance para vehículos tipo SUV y CrossOver.",
                        marca.MarcaRepuesto_ID,
                        subcat.SubCategoria_ID,
                        10000,
                        4,
                        1,
                        "Unidades"
                    );
                }, "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de Repuesto por Nombre.");
                
            });
            
        }

        [Test]
        public void CrearRepuestosParametrosNulos()
        {
            
            PopularRepuestos();
            var marca = MarcaRepuesto.GetMarca(1);
            var subcat = Categoria.GetCategoria(1).GetSubCategorias()[0];
            
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Repuesto.AgregarRepuesto(
                        null,
                        "Michelin Pilot Sport 4 SUV", 
                        "La llanta ultra high performance para vehículos tipo SUV y CrossOver.",
                        marca.MarcaRepuesto_ID,
                        subcat.SubCategoria_ID,
                        10000,
                        4,
                        1,
                        "Unidades"
                    );
                }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de Repuesto por un Codigo de Repuesto nulo.");
                
                Assert.Throws<ArgumentNullException>(() =>
                {
                    Repuesto.AgregarRepuesto(
                        "100-HF-120F",
                        null, 
                        "La llanta ultra high performance para vehículos tipo SUV y CrossOver.",
                        marca.MarcaRepuesto_ID,
                        subcat.SubCategoria_ID,
                        10000,
                        4,
                        1,
                        "Unidades"
                    );
                }, "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de Repuesto por nombre nulo.");
            });
            
        }
        
    }
}