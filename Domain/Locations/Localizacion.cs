using System;

namespace Domain.Locations
{
    public class Localizacion
    {

        private String Codigo { get; }
        private int Estante_ID { get; }
        
        public Localizacion() {}

        public Localizacion(String codigo)
        {
            this.Codigo = codigo;
        }

        public Localizacion(int estanteId, string codigo)
        {
            this.Codigo = codigo;
            this.Estante_ID = estanteId;
        }
        
        public override String ToString()
        {
            return $"Localizacion: [Codigo: {Codigo} / Estante ID: {Estante_ID}]";
        }

    }
}