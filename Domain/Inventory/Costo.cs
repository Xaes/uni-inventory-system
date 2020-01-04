using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Inventory
{
    public class Costo
    {

        public int Costo_ID, FK_Repuesto_ID, Unidades;
        public DateTime FechaEntrada;
        public float CostoUnitario;
        
        public Costo() {}

        private Costo(int costoId, int fkRepuestoId, int unidades, DateTime fechaEntrada, float costoUnitario)
        {
            this.Costo_ID = costoId;
            this.FK_Repuesto_ID = fkRepuestoId;
            this.Unidades = unidades;
            this.FechaEntrada = fechaEntrada;
            this.CostoUnitario = costoUnitario;
        }

        public static Costo AgregarCosto(int fkRepuestoId, int unidades, DateTime fechaEntrada, float costoUnitario)
        {
            
            // Validar si algun parametro esta fuera del minimo aceptable.

            if (costoUnitario < 0 || unidades <= 0)
                throw new ArgumentOutOfRangeException("", "Las Unidades deberian ser mayores a 0. " + 
                    "El Costo Unitario debe ser mayor o igual que 0.");
            
            try
            {
                const string sqlString = 
                    "Insert Into Costo (FK_Repuesto_ID, Unidades, FechaEntrada, CostoUnitario)" +
                    "Values (@fkRepuestoId, @unidades, @fechaEntrada, @costoUnitario); Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion()
                    .Execute(sqlString, new {fkRepuestoId, unidades, fechaEntrada, costoUnitario });
                
                return new Costo(id, fkRepuestoId, unidades, fechaEntrada, costoUnitario);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static List<Costo> GetCostos()
        {
            const string sqlString = "Select * From Costo";
            return DbCliente.GetConexion().Query<Costo>(sqlString).ToList();
        }

        public static Costo FindCosto(int costoId)
        {
            const string sqlString = "Select * From Costo Where Costo_ID = @costoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Costo>(sqlString, new { costoId });
        }

        public override string ToString()
        {
            return $"Costo: [ID: {Costo_ID} / Repuesto: {FK_Repuesto_ID} / Costo Unit.: {CostoUnitario}" +
                   $"Unidades: {Unidades} / Fecha de Entrada: {FechaEntrada}]";
        }
        
    }
}