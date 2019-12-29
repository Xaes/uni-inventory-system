using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Inventory
{
    public class Existencia
    {
        
        public int Existencia_ID, FK_Repuesto_ID, Unidades;
        public string FK_Localizacion_ID;
        
        public Existencia() {}

        private Existencia(int existenciaId, int fkRepuestoId, int unidades, string localizacionId)
        {
            this.Existencia_ID = existenciaId;
            this.FK_Repuesto_ID = fkRepuestoId;
            this.Unidades = unidades;
            this.FK_Localizacion_ID = localizacionId;
        }

        public static Existencia AgregarCosto(int fkRepuestoId, int unidades, string fkLocalizacionId)
        {
            try
            {
                const string sqlString = 
                    "Insert Into Existencia (FK_Repuesto_ID, Unidades, FK_Localizacion_ID)" +
                    "Values (@fkRepuestoId, @unidades, @fkLocalizacionId); Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion()
                    .Execute(sqlString, new {fkRepuestoId, unidades, fkLocalizacionId });
                
                return new Existencia(id, fkRepuestoId, unidades, fkLocalizacionId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static List<Existencia> GetExistencias()
        {
            const string sqlString = "Select * From Existencia";
            return DbCliente.GetConexion().Query<Existencia>(sqlString).ToList();
        }

        public static Existencia FindExistencia(int existenciaId)
        {
            const string sqlString = "Select * From Existencia Where Existencia_ID = @existenciaId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Existencia>(sqlString, new { existenciaId });
        }

        public override string ToString()
        {
            return $"Existencia: [ID: {Existencia_ID} / Repuesto: {FK_Repuesto_ID} / Localizacion: {FK_Localizacion_ID}" +
                   $"Unidades: {Unidades}]";
        }
        
    }
}