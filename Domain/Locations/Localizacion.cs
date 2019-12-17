using System;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Localizacion
    {

        private string Codigo { get; }
        private int Estante_ID { get; }
        
        public Localizacion() {}

        public Localizacion(string codigo)
        {
            this.Codigo = codigo;
        }

        public Localizacion(int estanteId, string codigo)
        {
            this.Codigo = codigo;
            this.Estante_ID = estanteId;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Localizacion (Codigo, Estante_ID) Values (@Codigo, @Estante_ID);";
            DbCliente.GetConexion().Execute(sqlString, new {this.Codigo, this.Estante_ID});
        }
        
        public override string ToString()
        {
            return $"Localizacion: [Codigo: {Codigo} / Estante ID: {Estante_ID}]";
        }

    }
}