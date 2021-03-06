using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Inventory
{

    public enum TipoTransaccion
    {
        ENTRADA,
        SALIDA
    }
    
    public class Movimiento
    {
        
        public int Movimiento_ID;
        public string FK_LocalizacionInicial_ID;
        public string FK_LocalizacionFinal_ID;
        public int FK_Periodo_ID;
        public float? CostoTotal, CostoUnitario, PrecioVentaUnitario;
        public int Unidades;
        public DateTime Fecha;
        public TipoTransaccion Transaccion;

        private Movimiento() {}

        public Movimiento(int movimientoId, string fkLocalizacionInicialId, string fkLocalizacionFinalId, int fkPeriodoId,
            float? costoTotal, float? costoUnitario, float? precioVentaUnitario,
            int unidades, DateTime fecha, TipoTransaccion transaccion
        )
        {
            this.Movimiento_ID = movimientoId;
            this.FK_LocalizacionInicial_ID = fkLocalizacionInicialId;
            this.FK_LocalizacionFinal_ID = fkLocalizacionFinalId;
            this.FK_Periodo_ID = fkPeriodoId;
            this.CostoTotal = costoTotal;
            this.CostoUnitario = costoUnitario;
            this.PrecioVentaUnitario = precioVentaUnitario;
            this.Unidades = unidades;
            this.Fecha = fecha;
            this.Transaccion = transaccion;
        }

        public static Movimiento AgregarMovimiento(string fkLocalizacionInicialId, string fkLocalizacionFinalId,
            int fkPeriodoId, float? costoUnitario, float? precioVentaUnitario,
            int unidades, DateTime fecha, TipoTransaccion transaccion
        )
        {
            
            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0 || costoUnitario < 0 || precioVentaUnitario < 0)
                throw new ArgumentOutOfRangeException("", "Las Unidades deberian ser mayores a 0. " + 
                    "El costo unitario y el precio de venta unitario debe ser mayor o igual que 0.");
            
            if (string.IsNullOrWhiteSpace(fkLocalizacionInicialId) && string.IsNullOrWhiteSpace(fkLocalizacionFinalId))
                throw new ArgumentNullException("","Ambas Localizaciones no pueden ser nulas, estar vacias o solo contener espacios.");
            
            if (string.CompareOrdinal(fkLocalizacionFinalId, fkLocalizacionInicialId) == 0)
                throw new ArgumentException("Un Movimiento no puede tener la misma localizacion como inicial y final.");
            
            var periodo = Periodo.GetPeriodoActivo();
            
            // Checkear si existe un periodo activo.
            
            if(periodo == null)
                throw new InvalidOperationException("No hay un periodo activo.");
                
            // Checkear si la fecha proporcionada esta dentro del rango del periodo activo.
            
            if (periodo.FechaInicio.CompareTo(fecha) > 0 || periodo.FechaFinal.CompareTo(fecha) < 0)
                throw new ArgumentException("La fecha propocionada debe estar dentro del rango del periodo activo.");
            
            try
            {
                
                var tipoTransaccion = transaccion.ToString();
                var costoTotal = costoUnitario * unidades;
                
                const string sqlString = 
                    "Insert Into Movimiento (FK_LocalizacionInicial_ID, FK_LocalizacionFinal_ID, FK_Periodo_ID," +
                    " CostoTotal, CostoUnitario, PrecioVentaUnitario, Unidades, Fecha, TipoTransaccion)" +
                    "Values (@fkLocalizacionInicialId, @fkLocalizacionFinalId, @fkPeriodoId, @costoTotal," +
                    "@costoUnitario, @precioVentaUnitario, @unidades, @fecha, @tipoTransaccion);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new
                {
                    fkLocalizacionInicialId, fkLocalizacionFinalId, fkPeriodoId,
                    costoTotal, costoUnitario, precioVentaUnitario, unidades, fecha, tipoTransaccion
                });
                
                return new Movimiento(id, fkLocalizacionInicialId, fkLocalizacionFinalId, fkPeriodoId,
                    costoTotal, costoUnitario, precioVentaUnitario, unidades, fecha, transaccion);
                
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static Movimiento FindMovimiento(int movimientoId)
        {
            const string sqlString = "Select * From Movimiento Where Movimiento_ID = @movimientoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Movimiento>(sqlString, new {movimientoId});
        }

        public static List<Movimiento> GetMovimientos()
        {
            const string sqlString = "Select * From Movimiento";
            return DbCliente.GetConexion().Query<Movimiento>(sqlString).ToList();
        }

        public override string ToString()
        {
            return $"Movimiento: [ID: {Movimiento_ID} / Local. Inicial: {FK_LocalizacionInicial_ID} / " +
                   $"Local. Final: {FK_LocalizacionFinal_ID} / Periodo: {FK_Periodo_ID} / Costo Total: {CostoTotal} / " +
                   $"Costo Unitario: {CostoUnitario} / Precio Venta Unitario: {PrecioVentaUnitario} / " +
                   $"Unidades: {Unidades} / Fecha: {Fecha} / Tipo Transaccion: {Transaccion}]";
        }
        
    }
}