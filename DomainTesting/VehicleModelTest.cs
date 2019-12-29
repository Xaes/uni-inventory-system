using System;
using Domain.Products;
using NUnit.Framework;

namespace DomainTesting
{
    public class VehicleModelTest : Setup
    {

        public static void PopularModelos()
        {
            ModeloVehiculo.AgregarModelo("RAV4", "Toyota", "Eco Sport");
        }

        [Test]
        public void CrearModelos()
        {
            Assert.DoesNotThrow(() =>
            {
                PopularModelos();
                ModeloVehiculo.FindModelo(1);
                ModeloVehiculo.GetModelos();
            });
        }

        [Test]
        public void CrearModelosParametrosNulos()
        {
            Assert.Multiple(() =>
                {
                    Assert.Catch<ArgumentNullException>(() => 
                        ModeloVehiculo.AgregarModelo(null, "Toyota", "Eco Sport"),
                        "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de ModeloVehiculo."
                    );
                    
                    Assert.Catch<ArgumentNullException>(() => 
                        ModeloVehiculo.AgregarModelo("RAV4", null, "Eco Sport"),
                        "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de ModeloVehiculo."
                    );
                    
                    Assert.DoesNotThrow(() => 
                        ModeloVehiculo.AgregarModelo("RAV4", "Toyota", null),
                        "[ERROR]: Una excepcion ArgumentNullException deberia ser lanzada en creacion de ModeloVehiculo."
                    );
                }
            );
        }
    }
}