using System.Data;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class CompatibilityTest : Setup
    {

        public void PopularCompatibilidad()
        {
            VehicleModelTest.PopularModelos();
            ReplacementTest.PopularRepuestos();

            var repuesto = Repuesto.FindRepuesto(1);
            var modelo = ModeloVehiculo.FindModelo(1);
            
            RepuestoCompatibilidad.AddCompatibilidad(repuesto.Repuesto_ID, modelo.ModeloVehiculo_ID);
        }

        [Test]
        public void CrearCompatibilidad()
        {
            Assert.DoesNotThrow(() =>
            {
                this.PopularCompatibilidad();
                RepuestoCompatibilidad.GetCompatibilidades();
                RepuestoCompatibilidad.FindCompatbilidadByRepuesto(1);
                RepuestoCompatibilidad.FindCompatibilidadByModelo(1);
            });
        }

        [Test]
        public void DuplicarCompatibilidad()
        {
            this.PopularCompatibilidad();
            Assert.Throws<DuplicateNameException>(
                this.PopularCompatibilidad,
                "[ERROR]: Una excepcion DuplicateNameException deberia ser lanzada en creacion de VehiculoCompatibilidad."
            );
        }
    }
}