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

        public int Repuesto_ID;
        public int FK_MarcaRepuesto_ID;
        public int FK_SubCategoria_ID;
        public int NumeroParte;
        public int? UnidadesPorPaquete, UnidadesEmpaque;
        public string UnidadMedida, Nombre, Descripcion, CodigoRepuesto;
        
        public Repuesto() {}

        public Repuesto(int repuestoId, string codigoRepuesto, string nombre, string descripcion,
            int fkMarcaRepuestoId, int fkSubCategoriaId, int numeroParte, int? unidadesPorPaquete, int? unidadesEmpaque,
            string unidadMedida)
        {
            this.Repuesto_ID = repuestoId;
            this.CodigoRepuesto = codigoRepuesto;
            this.FK_MarcaRepuesto_ID = fkMarcaRepuestoId;
            this.FK_SubCategoria_ID = fkSubCategoriaId;
            this.NumeroParte = numeroParte;
            this.UnidadesEmpaque = unidadesEmpaque;
            this.UnidadesPorPaquete = unidadesPorPaquete;
            this.UnidadMedida = unidadMedida;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public static Repuesto AgregarRepuesto(string codigoRepuesto, string nombre, string descripcion, 
            int fkMarcaRepuestoId, int fkSubCategoriaId, int numeroParte, int? unidadesPorPaquete, 
            int? unidadesEmpaque, string unidadMedida)
        {
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");
            
            if(string.IsNullOrWhiteSpace(codigoRepuesto))
                throw new ArgumentNullException(nameof(codigoRepuesto), "Codigo de Repuesto no puede ser nulo, estar vacio o solo contener espacios.");

            try
            {
                const string sqlString =
                    "Insert Into Repuesto (Nombre, CodigoRepuesto, Descripcion, FK_MarcaRepuesto_ID, " +
                     "FK_SubCategoria_ID, NumeroParte, UnidadesPorPaquete, UnidadesEmpaque, UnidadMedida)" +
                     "Values (@nombre, @codigoRepuesto, @descripcion, @fkMarcaRepuestoId," +
                     "@fkSubCategoriaId, @numeroParte, @unidadesPorPaquete, @unidadesEmpaque, @unidadMedida);" +
                     "Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString,
                    new
                    {
                        nombre, codigoRepuesto, descripcion, fkMarcaRepuestoId, fkSubCategoriaId, numeroParte, 
                        unidadesPorPaquete, unidadesEmpaque, unidadMedida
                    });

                return new Repuesto(id, codigoRepuesto, nombre, descripcion, fkMarcaRepuestoId, 
                    fkSubCategoriaId, numeroParte, unidadesPorPaquete, unidadesEmpaque, unidadMedida);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static Repuesto FindRepuesto(int repuestoId)
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