using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Estante
    {
        private int Estante_ID { get; }
        private int Pasillo_ID { get; }
        private int Secuencia_Loc { get; }
        private string Codigo { get; }
        
        public Estante() {}

        public Estante(string codigo, int pasilloId, int secuenciaLoc)
        {
            this.Codigo = codigo;
            this.Pasillo_ID = pasilloId;
            this.Secuencia_Loc = secuenciaLoc;
        }
        
        public Estante(int estanteId, string codigo, int pasilloId, int secuenciaLoc)
        {
            this.Estante_ID = estanteId;
            this.Codigo = codigo;
            this.Pasillo_ID = pasilloId;
            this.Secuencia_Loc = secuenciaLoc;
        }

        public List<Localizacion> GetLocalizaciones()
        {
            return DbCliente.GetConexion()
                .Query<Localizacion>("Select * from Localizacion Where Estante_ID = @Estante_ID", new {Estante_ID})
                .ToList();
        }

        public override string ToString()
        {
            return $"Estante: [Codigo: {Codigo} / ID: {Estante_ID} / Pasillo ID: {Pasillo_ID}]";
        }

    }
}