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
        private int Secuencia_Loc { get; set; }
        public string Codigo { get; }
        
        public Estante() {}

        internal Estante(string codigo, int pasilloId, int secuenciaLoc)
        {
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.Pasillo_ID = pasilloId;
            this.Secuencia_Loc = secuenciaLoc;
        }

        public List<Localizacion> GetLocalizaciones()
        {
            const string sqlString = "Select * from Localizacion Where Estante_ID = @Estante_ID";
            return DbCliente.GetConexion().Query<Localizacion>(sqlString, new {Estante_ID}).ToList();
        }

        public Pasillo GetPasillo()
        {
            const string sqlString = "Select * from Pasillo Where Pasillo_ID = @Pasillo_ID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Pasillo>(sqlString, new {this.Pasillo_ID});
        }

        public Localizacion AgregarLocalizacion()
        {
            
            // Creando Localizacion con codigo generado.
            
            var localizacion = new Localizacion(this.Estante_ID, this.GenerarCodigo());
            
            // Actualizando Secuencia Loc.
            
            this.ActualizarSecuencia();

            // Guardando Localizacion.

            localizacion.Insertar();
            return localizacion;
            
        }

        public string GenerarCodigo()
        {
            var pasillo = this.GetPasillo();
            var bodega = pasillo.GetBodega();
            return this.Secuencia_Loc + "-" + this.Codigo + "-" + pasillo.Codigo + "-" + bodega.Codigo;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Estante (Codigo, Pasillo_ID, Secuencia_Loc) Values (@Codigo, @Pasillo_ID, @Secuencia_Loc);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new {this.Codigo, this.Pasillo_ID, this.Secuencia_Loc});
            this.Estante_ID = id;
        }

        private void ActualizarSecuencia()
        {
            this.Secuencia_Loc++;
            const string sqlString = "Update Estante Set Secuencia_Loc = @Secuencia_Loc Where Estante_ID = @Estante_ID";
            DbCliente.GetConexion().Execute(sqlString, new { this.Secuencia_Loc, this.Estante_ID });
        }

        public override string ToString()
        {
            return $"Estante: [Codigo: {Codigo} / ID: {Estante_ID} / Pasillo ID: {Pasillo_ID}]";
        }

    }
}