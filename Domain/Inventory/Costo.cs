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

        public static List<Dictionary<string, dynamic>> ProyectarExtraccion(int repuestoId, int cantidad)
        {
            
            if(cantidad < 1 )
                throw new ArgumentOutOfRangeException(nameof(cantidad), "Cantidad a extraer no puede ser menor a 1.");
            
            // Trayendo Costos ordenados por Fecha de Entrada.
            
            const string sqlString = "SELECT * FROM Costo WHERE FK_Repuesto_ID = @repuestoId ORDER BY FechaEntrada ASC";
            var costos = DbCliente.GetConexion().Query<Costo>(sqlString, new { repuestoId }).ToList();
            
            // Validar si la lista de Costos esta vacia.
            
            if(costos.Count < 1)
                throw new InvalidOperationException("No existen costos para este producto.");
            
            // Definiendo Estructura de un Resultado (Datos de Proyeccion).
            
            var costosExtraidos = new List<Dictionary<string, dynamic>>();
            
            // Lambda para agregar a la Proyeccion.
            
            Action<int, int, float> crearDict = (id, c, costoUnitario) =>
            {
                costosExtraidos.Add(new Dictionary<string, dynamic>
                {
                    {"id", id},
                    {"cantidad", c},
                    {"costoUnitario", costoUnitario}
                });
            };
            
            // Extrayendo Costos.

            foreach (var costo in costos)
            {
                if (cantidad - costo.Unidades > 0)
                {
                    cantidad -= costo.Unidades;
                    crearDict(costo.Costo_ID, costo.Unidades, costo.CostoUnitario);
                } else {
                    crearDict(costo.Costo_ID, (cantidad > costo.Unidades) ? costo.Unidades : cantidad, costo.CostoUnitario);
                    break;
                }
            }

            return costosExtraidos;
        }

        public static List<Dictionary<string, dynamic>> ExtraerCosto(int repuestoId, int cantidad)
        {
            var proyeccion = Costo.ProyectarExtraccion(repuestoId, cantidad);
            proyeccion.ForEach(p => Costo.FindCosto(p["id"]).ExtraerUnidades(p["cantidad"]));
            return proyeccion;
        }
        
        private Costo ExtraerUnidades(int cantidad)
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
            const string sqlString = "Update Costo Set Unidades = @nuevaCantidad Where Costo_ID = @Costo_ID;";
            DbCliente.GetConexion().Execute(sqlString, new { nuevaCantidad, this.Costo_ID });
            this.Unidades = nuevaCantidad;
            return this;
            
        }

        private void Eliminar()
        {
            if(this.Unidades != 0)
                throw new InvalidOperationException("No se puede eliminar un costo que tiene todavia unidades presentes.");
            
            const string sqlString = "Delete From Costo Where Costo_ID = @Costo_ID";
            DbCliente.GetConexion().Execute(sqlString, new { this.Costo_ID });
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
            return $"Costo: [ID: {Costo_ID} / Repuesto: {FK_Repuesto_ID} / Costo Unit.: {CostoUnitario} / " +
                   $"Unidades: {Unidades} / Fecha de Entrada: {FechaEntrada}]";
        }
        
    }
}