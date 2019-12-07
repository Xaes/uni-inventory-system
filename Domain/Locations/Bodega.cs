using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Bodega
    {
        
        private int Bodega_ID { get; }
        private string Nombre { get; }
        private string Codigo { get; }
        
        public Bodega() {}

        public Bodega(string nombre, string codigo)
        {
            this.Nombre = nombre;
            this.Codigo = codigo;
        }

        public Bodega(int bodegaId, string codigo, string nombre)
        {
            this.Bodega_ID = bodegaId;
            this.Nombre = nombre;
            this.Codigo = codigo;
        }

        public List<Pasillo> GetPasillos()
        {
            return DbCliente.GetConexion()
                .Query<Pasillo>("Select * From Pasillo Where Bodega_ID = @Bodega_ID", new {this.Bodega_ID})
                .ToList();
        }

        public override string ToString()
        {
            return $"Bodega: [Codigo: {Codigo} / Nombre: {Nombre} / ID: {Bodega_ID}]";
        }
    }
}