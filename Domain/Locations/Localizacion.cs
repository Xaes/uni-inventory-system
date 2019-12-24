using System;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Localizacion
    {

        private string Codigo { get; }
        private int FK_Estante_ID { get; }
        
        public Localizacion() {}

        internal Localizacion(int estanteId, string codigo)
        {
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.FK_Estante_ID = estanteId;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Localizacion (Codigo, FK_Estante_ID) Values (@Codigo, @FK_Estante_ID);";
            DbCliente.GetConexion().Execute(sqlString, new {this.Codigo, this.FK_Estante_ID});
        }
        
        public override string ToString()
        {
            return $"Localizacion: [Codigo: {Codigo} / Estante ID: {FK_Estante_ID}]";
        }

    }
}