using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Products
{
    public class RepuestoCompatibilidad
    {
        
        public int FK_Repuesto_ID, FK_ModeloVehiculo_ID;
        
        public RepuestoCompatibilidad(){}

        private RepuestoCompatibilidad(int fkRepuestoId, int fkModeloVehiculoId)
        {
            this.FK_Repuesto_ID = fkRepuestoId;
            this.FK_ModeloVehiculo_ID = fkModeloVehiculoId;
        }

        public static RepuestoCompatibilidad AddCompatibilidad(int fkRepuestoId, int fkModeloVehiculoId)
        {
            try
            {
                const string sqlString =  
                    "Insert Into RepuestoCompatibilidad (FK_Repuesto_ID, FK_ModeloVehiculo_ID)" +
                    "Values (@fkRepuestoId, @fkModeloVehiculoId);";

                DbCliente.GetConexion().Execute(sqlString, new {fkRepuestoId, fkModeloVehiculoId});
                return new RepuestoCompatibilidad(fkRepuestoId, fkModeloVehiculoId);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static List<RepuestoCompatibilidad> FindCompatbilidadByRepuesto(int fkRepuestoId)
        {
            const string sqlString = "Select * From RepuestoCompatibilidad Where FK_Repuesto_ID = @fkRepuestoId";
            return DbCliente.GetConexion().Query<RepuestoCompatibilidad>(sqlString, new {fkRepuestoId}).ToList();
        }
        
        public static List<RepuestoCompatibilidad> FindCompatibilidadByModelo(int fkModeloId)
        {
            const string sqlString = "Select * From RepuestoCompatibilidad Where FK_ModeloVehiculo_ID = @fkModeloId";
            return DbCliente.GetConexion().Query<RepuestoCompatibilidad>(sqlString, new {fkModeloId}).ToList();
        }

        public static List<RepuestoCompatibilidad> GetCompatibilidades()
        {
            const string sqlString = "Select * From RepuestoCompatibilidad";
            return DbCliente.GetConexion().Query<RepuestoCompatibilidad>(sqlString).ToList();
        }

        public override string ToString()
        {
            return $"Compatibilidad de Repuesto: [Repuesto: {FK_Repuesto_ID} / Modelo de Vehiculo: {FK_ModeloVehiculo_ID}]";
        }
    }
}