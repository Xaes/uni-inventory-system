using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Locations
{
    public class Estante
    {
        private int Estante_ID { get; set; }
        private int FK_Pasillo_ID { get; }
        private int Secuencia_Loc { get; set; }
        public string Codigo { get; }
        
        public Estante() {}

        internal Estante(string codigo, int pasilloId, int secuenciaLoc)
        {
            
            if(string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentNullException(nameof(codigo), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");
            
            this.Codigo = codigo;
            this.FK_Pasillo_ID = pasilloId;
            this.Secuencia_Loc = secuenciaLoc;
            
        }

        public List<Localizacion> GetLocalizaciones()
        {
            const string sqlString = "Select * from Localizacion Where FK_Estante_ID = @Estante_ID";
            return DbCliente.GetConexion().Query<Localizacion>(sqlString, new {Estante_ID}).ToList();
        }

        public Pasillo GetPasillo()
        {
            const string sqlString = "Select * from Pasillo Where Pasillo_ID = @FK_Pasillo_ID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Pasillo>(sqlString, new {this.FK_Pasillo_ID});
        }

        public Localizacion AgregarLocalizacion()
        {
            try {
                
                // Creando Localizacion con codigo generado.
                var localizacion = new Localizacion(this.Estante_ID, this.GenerarCodigo());
                
                // Actualizando Secuencia Loc.
                this.ActualizarSecuencia();

                // Guardando Localizacion.
                localizacion.Insertar();
                return localizacion;
                
            } catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public string GenerarCodigo()
        {
            var pasillo = this.GetPasillo();
            var bodega = pasillo.GetBodega();
            return this.Secuencia_Loc + "-" + this.Codigo + "-" + pasillo.Codigo + "-" + bodega.Codigo;
        }

        public void Insertar()
        {
            const string sqlString = "Insert Into Estante (Codigo, FK_Pasillo_ID, Secuencia_Loc) Values (@Codigo, @FK_Pasillo_ID, @Secuencia_Loc);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new {this.Codigo, this.FK_Pasillo_ID, this.Secuencia_Loc});
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
            return $"Estante: [Codigo: {Codigo} / ID: {Estante_ID} / Pasillo ID: {FK_Pasillo_ID}]";
        }

    }
}