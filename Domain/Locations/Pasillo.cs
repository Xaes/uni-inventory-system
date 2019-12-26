using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Pasillo
    {
        
        public int Pasillo_ID { get; private set; }
        private int FK_Bodega_ID { get; }
        public string Codigo { get; }
        
        public Pasillo() {}

        public Pasillo(string codigo, int bodega_ID)
        {
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.FK_Bodega_ID = bodega_ID;
        }

        public List<Estante> GetEstantes()
        {
            const string sqlString = "Select * from Estante Where FK_Pasillo_ID = @Pasillo_ID";
            return DbCliente.GetConexion().Query<Estante>(sqlString, new { Pasillo_ID }).ToList();
        }

        public Estante AgregarEstante(string codigo, int secuenciaLoc)
        {
            var estante = new Estante(codigo, this.Pasillo_ID, secuenciaLoc);
            estante.Insertar();
            return estante;
        }

        public Bodega GetBodega()
        {
            const string sqlString = "Select * from Bodega Where Bodega_ID = @FK_Bodega_ID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Bodega>(sqlString, new { this.FK_Bodega_ID });
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Pasillo (Codigo, FK_Bodega_ID) Values (@Codigo, @FK_Bodega_ID);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            this.Pasillo_ID = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new { this.Codigo, this.FK_Bodega_ID });
        }

        public override String ToString()
        {
            return $"Pasillo: [Codigo: {Codigo} / ID: {Pasillo_ID} / Bodega ID: {FK_Bodega_ID}]";
        }
    }
}