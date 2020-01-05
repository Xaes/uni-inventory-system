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

        public Existencia()
        {
        }

        private Existencia(int existenciaId, int fkRepuestoId, int unidades, string localizacionId)
        {
            this.Existencia_ID = existenciaId;
            this.FK_Repuesto_ID = fkRepuestoId;
            this.Unidades = unidades;
            this.FK_Localizacion_ID = localizacionId;
        }

        public static Existencia AgregarExistencia(int fkRepuestoId, int unidades, string fkLocalizacionId)
        {

            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0)
                throw new ArgumentOutOfRangeException("", "Las Unidades deberian ser mayores a 0. " +
                    "El Costo Unitario debe ser mayor o igual que 0.");

            try
            {
                const string sqlString =
                    "Insert Into Existencia (FK_Repuesto_ID, Unidades, FK_Localizacion_ID)" +
                    "Values (@fkRepuestoId, @unidades, @fkLocalizacionId); Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion()
                    .Execute(sqlString, new { fkRepuestoId, unidades, fkLocalizacionId });

                return new Existencia(id, fkRepuestoId, unidades, fkLocalizacionId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }

        }

        public Existencia ExtraerUnidades(int cantidad)
        {
            
            if(cantidad > this.Unidades)
                throw new InvalidOperationException("No se pueden extraer mas unidades que las existentes.");

            if (cantidad == this.Unidades)
            {
                this.Unidades = 0;
                this.Eliminar();
                return this;
            }
            
            var nuevaCantidad = this.Unidades - cantidad;
            const string sqlString = "Update Existencia Set Unidades = @nuevaCantidad Where Existencia_ID = @Existencia_ID;";
            DbCliente.GetConexion().Execute(sqlString, new { nuevaCantidad, this.Existencia_ID });
            this.Unidades = nuevaCantidad;
            return this;
            
        }
        
        public Existencia AgregarUnidades(int cantidad)
        {
            
            // Checkear si cantidad proporcionada es menor a 1.
            
            if(cantidad < 1)
                throw new ArgumentOutOfRangeException(nameof(cantidad), "Cantidad debe ser mayor a 0. Para extraer, use el metodo Extraer Unidades");
            
            var nuevaCantidad = cantidad + this.Unidades;
            const string sqlString = "Update Existencia Set Unidades = @nuevaCantidad Where Existencia_ID = @Existencia_ID;";
            DbCliente.GetConexion().Execute(sqlString, new { nuevaCantidad, this.Existencia_ID });
            this.Unidades = nuevaCantidad;
            return this;
            
        }

        private void Eliminar()
        {
            if(this.Unidades != 0)
                throw new InvalidOperationException("No se puede eliminar una existencia que tiene todavia unidades presentes.");
            
            const string sqlString = "Delete From Existencia Where Existencia_ID = @Existencia_ID";
            DbCliente.GetConexion().Execute(sqlString, new { this.Existencia_ID });
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

        public static List<Existencia> FindExistenciasByRepuesto(int repuestoId)
        {
            const string sqlString = "Select * From Existencia Where FK_Repuesto_ID = @FK_Repuesto_ID";
            return DbCliente.GetConexion().Query<Existencia>(sqlString, new { repuestoId }).ToList();
        }

        public static Existencia FindExistenciaByRepLoc(int repuestoId, string localizacionId)
        {
            const string sqlString = "Select * From Existencia Where FK_Repuesto_ID = @repuestoId And" +
                                     "FK_Localizacion_ID = @FK_Localizacion_ID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Existencia>(sqlString, new { repuestoId, localizacionId });
        }

        public override string ToString()
        {
            return $"Existencia: [ID: {Existencia_ID} / Repuesto: {FK_Repuesto_ID} / Localizacion: {FK_Localizacion_ID}" +
                   $"Unidades: {Unidades}]";
        }
        
    }
}