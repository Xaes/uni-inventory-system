using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Inventory
{
    public enum PeriodoEstado {
        ABIERTO,
        CERRADO,
        NOUSADO
    }
    
    public class Periodo
    {

        public int Periodo_ID { get; }
        public string Nombre { get; }
        public int Periodo_Fiscal { get; }
        public DateTime FechaInicio { get; }
        public DateTime FechaFinal { get; }
        public PeriodoEstado Estado;
        
        public Periodo() {}
        
        private Periodo(int periodoId, string nombre, int periodoFiscal, DateTime fechaInicio, DateTime fechaFinal, PeriodoEstado estado)
        {
            this.Periodo_ID = periodoId;
            this.Estado = estado;
            this.Nombre = nombre;
            this.Periodo_Fiscal = periodoFiscal;
            this.FechaInicio = fechaInicio;
            this.FechaFinal = fechaFinal;
        }

        public static Periodo AgregarPeriodo(int periodoFiscal, string nombre)
        {

            if(FindByPeriodoFiscal(periodoFiscal) != null)
                throw new ArgumentException("Periodo Fiscal ya existe.");
            
            var fechaInicio = new DateTime(periodoFiscal, 1, 1);
            var fechaFinal = new DateTime(periodoFiscal, 12, 31);
            var estadoString = PeriodoEstado.NOUSADO.ToString();
            
            const string sqlString = "Insert Into Periodo (Periodo_Fiscal, Nombre, FechaInicio, FechaFinal, Estado)" +
                                     "Values (@periodoFiscal, @nombre, @fechaInicio, @fechaFinal, @estadoString);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(
                sqlString,
                new { periodoFiscal, nombre, fechaInicio, fechaFinal, estadoString }
            );

            return new Periodo(id, nombre, periodoFiscal, fechaInicio, fechaFinal, PeriodoEstado.NOUSADO);

        }

        public static Periodo GetPeriodo(int periodoFiscal)
        {
            const string sqlString = "Select * From Periodo Where Periodo_Fiscal = @periodoFiscal";
            return DbCliente.GetConexion().QueryFirstOrDefault<Periodo>(sqlString, new { periodoFiscal });
        }

        public static List<Periodo> GetPeriodos()
        {
            const string sqlString = "Select * From Periodo";
            return DbCliente.GetConexion().Query<Periodo>(sqlString).ToList();
        }

        public static Periodo GetPeriodoActivo()
        {
            const string sqlString = "Select * From Periodo Where Estado = @estado";
            var estado = PeriodoEstado.ABIERTO.ToString();
            return DbCliente.GetConexion().QueryFirstOrDefault<Periodo>(sqlString, new { estado });
        }

        public static Periodo FindByPeriodoFiscal(int year)
        {
            const string sqlString = "Select * From Periodo Where Periodo_Fiscal = @year";
            return DbCliente.GetConexion().QueryFirstOrDefault<Periodo>(sqlString, new { year });
        }

        public void Abrir()
        {
            if (GetPeriodoActivo() != null)
                throw new ArgumentException("Ya existe un periodo fiscal activo. Abrir otro no es posible.");
            if (this.Estado != PeriodoEstado.NOUSADO)
                throw new ArgumentException("Solo se pueden abrir periodos sin usar.");
            
            this.CambiarEstado(PeriodoEstado.ABIERTO);
        }

        public void Cerrar()
        {
            if (this.Estado != PeriodoEstado.ABIERTO)
                throw new ArgumentException("Solo se puede cerrar un periodo abierto.");
            this.CambiarEstado(PeriodoEstado.CERRADO);
        }

        private void CambiarEstado(PeriodoEstado nuevoEstado)
        {
            const string sqlString = "Update Periodo Set Estado = @estado Where Periodo_ID = @Periodo_ID";
            var estado = nuevoEstado.ToString();
            DbCliente.GetConexion().Execute(sqlString, new { estado, this.Periodo_ID });
            this.Estado = nuevoEstado;
        }

        public override string ToString()
        {
            return $"Periodo: [ID: {Periodo_ID} / Periodo Fiscal: {Periodo_Fiscal} / Nombre: {Nombre} / " +
                   $"Fecha Inicio: {FechaInicio} / Fecha Final: {FechaFinal} / Estado: {Estado}]";
        }

    }
}