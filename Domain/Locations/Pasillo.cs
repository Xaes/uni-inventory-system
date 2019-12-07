using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Pasillo
    {
        
        private int Pasillo_ID { get; }
        private int Bodega_ID { get; }
        private string Codigo { get; }
        
        public Pasillo() {}

        public Pasillo(string codigo, int bodega_ID)
        {
            this.Codigo = codigo;
            this.Bodega_ID = bodega_ID;
        }

        public Pasillo(string codigo, int pasilloId, int bodegaId)
        {
            this.Codigo = codigo;
            this.Pasillo_ID = pasilloId;
            this.Bodega_ID = bodegaId;
        }

        public List<Estante> GetEstantes()
        {
            return DbCliente.GetConexion()
                .Query<Estante>("Select * from Estante Where Pasillo_ID = @Pasillo_ID", new { Pasillo_ID })
                .ToList();
        }

        public override String ToString()
        {
            return $"Pasillo: [Codigo: {Codigo} / ID: {Pasillo_ID} / Bodega ID: {Bodega_ID}]";
        }
    }
}