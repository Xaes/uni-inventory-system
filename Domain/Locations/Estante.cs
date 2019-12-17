using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Estante
    {
        private int Estante_ID { get; set; }
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
            const string sqlString = "Select * from Localizacion Where Estante_ID = @Estante_ID";
            return DbCliente.GetConexion().Query<Localizacion>(sqlString, new {Estante_ID}).ToList();
        }

        public Localizacion AgregarLocalizacion(string codigo)
        {
            var localizacion = new Localizacion(this.Estante_ID, codigo);
            localizacion.Insertar();
            return localizacion;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Estante (Codigo, Pasillo_ID, Secuencia_Loc) Values (@Codigo, @Pasillo_ID, @Secuencia_Loc);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new {this.Codigo, this.Pasillo_ID, this.Secuencia_Loc});
            this.Estante_ID = id;
        }

        public override string ToString()
        {
            return $"Estante: [Codigo: {Codigo} / ID: {Estante_ID} / Pasillo ID: {Pasillo_ID}]";
        }

    }
}