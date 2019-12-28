using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Products
{
    public class Repuesto
    {

        private int Repuesto_ID;
        private int FK_MarcaRepuesto_ID;
        private int FK_SubCategoria_ID;
        private int NumeroParte;
        private int? UnidadesPorPaquete, UnidadesEmpaque;
        private string UnidadMedida, Nombre, Descripcion;
        
        public Repuesto() {}

        public Repuesto(int repuestoId, string nombre, string descripcion, int fkMarcaRepuestoId, int fkSubCategoriaId,
            int numeroParte, int? unidadesPorPaquete, int? unidadesEmpaque, string unidadMedida)
        {
            this.Repuesto_ID = repuestoId;
            this.FK_MarcaRepuesto_ID = fkMarcaRepuestoId;
            this.FK_SubCategoria_ID = fkSubCategoriaId;
            this.NumeroParte = numeroParte;
            this.UnidadesEmpaque = unidadesEmpaque;
            this.UnidadesPorPaquete = unidadesPorPaquete;
            this.UnidadMedida = unidadMedida;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public static Repuesto AgregarRepuesto(string nombre, string descripcion, int fkMarcaRepuestoId,
            int fkSubCategoriaId, int numeroParte, int? unidadesPorPaquete, int? unidadesEmpaque, string unidadMedida)
        {
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");

            try
            {
                const string sqlString =
                    "Insert Into Repuesto (Nombre, Descripcion, FK_MarcaRepuesto_ID, " +
                     "FK_SubCategoria_ID, NumeroParte, UnidadesPorPaquete, UnidadesEmpaque, UnidadMedida)" +
                     "Values (@nombre, @descripcion, @fkMarcaRepuestoId," +
                     "@fkSubCategoriaId, @numeroParte, @unidadesPorPaquete, @unidadesEmpaque, @unidadMedida);" +
                     "Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString,
                    new
                    {
                        nombre, descripcion, fkMarcaRepuestoId, fkSubCategoriaId, numeroParte, unidadesPorPaquete,
                        unidadesEmpaque, unidadMedida
                    });

                return new Repuesto(id, nombre, descripcion, fkMarcaRepuestoId, 
                    fkSubCategoriaId, numeroParte, unidadesPorPaquete, unidadesEmpaque, unidadMedida);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static Repuesto GetRepuesto(int repuestoId)
        {
            const string sqlString = "Select * From Repuesto Where Repuesto_ID = @repuestoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Repuesto>(sqlString, new {repuestoId});
        }

        public static List<Repuesto> GetRepuestos()
        {
            const string sqlString = "Select * From Repuesto";
            return DbCliente.GetConexion().Query<Repuesto>(sqlString).ToList();
        }

        public override string ToString()
        {
            return $"Repuesto: [ID: {Repuesto_ID} / Nombre: {Nombre} / Descripcion: {Descripcion} / " +
                   $"Marca Repuesto: {FK_MarcaRepuesto_ID} / SubCategoria: {FK_SubCategoria_ID} / " +
                   $"No. Parte: {NumeroParte} / U. Empaque: {UnidadesEmpaque} / U. x Paquete: {UnidadesPorPaquete} / " +
                   $"Unidad de Medida: {UnidadMedida}]";
        }
    }
}