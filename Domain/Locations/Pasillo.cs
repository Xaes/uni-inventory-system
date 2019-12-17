using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Pasillo
    {
        
        private int Pasillo_ID { get; set; }
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
            const string sqlString = "Select * from Estante Where Pasillo_ID = @Pasillo_ID";
            return DbCliente.GetConexion().Query<Estante>(sqlString, new { Pasillo_ID }).ToList();
        }

        public Estante AgregarEstante(string codigo, int secuenciaLoc)
        {
            var estante = new Estante(codigo, this.Pasillo_ID, secuenciaLoc);
            estante.Insertar();
            return estante;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Pasillo (Codigo, Bodega_ID) Values (@Codigo, @Bodega_ID);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            this.Pasillo_ID = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new { this.Codigo, this.Bodega_ID });
        }

        public override String ToString()
        {
            return $"Pasillo: [Codigo: {Codigo} / ID: {Pasillo_ID} / Bodega ID: {Bodega_ID}]";
        }
    }
}