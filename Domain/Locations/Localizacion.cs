using System;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Locations
{
    public class Localizacion
    {

        private string Codigo { get; }
        private int FK_Estante_ID { get; }
        
        public Localizacion() {}

        internal Localizacion(int estanteId, string codigo)
        {
            
            if(string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentNullException(nameof(codigo), "Codigo no puede ser nulo, estar vacio o solo contener espacios.");
            
            this.Codigo = codigo;
            this.FK_Estante_ID = estanteId;
            
        }

        public void Insertar()
        {
            try
            {
                const string sqlString =
                    "Insert Into Localizacion (Codigo, FK_Estante_ID) Values (@Codigo, @FK_Estante_ID);";
                DbCliente.GetConexion().Execute(sqlString, new {this.Codigo, this.FK_Estante_ID});
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }
        
        public override string ToString()
        {
            return $"Localizacion: [Codigo: {Codigo} / Estante ID: {FK_Estante_ID}]";
        }

    }
}