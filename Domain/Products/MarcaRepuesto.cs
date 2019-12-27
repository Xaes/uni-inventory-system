using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Products
{
    public class MarcaRepuesto
    {
        
        public int MarcaRepuesto_ID { get; }
        private bool EsGenerica;
        private string Nombre;
        
        public MarcaRepuesto() {}

        private MarcaRepuesto(int marcaRepuestoId, string nombre, bool esGenerica)
        {
            this.MarcaRepuesto_ID = marcaRepuestoId;
            this.Nombre = nombre;
            this.EsGenerica = esGenerica;
        }

        public static MarcaRepuesto AgregarMarca(string nombre, bool esGenerica)
        {
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");

            try
            {
                const string sqlString = "Insert Into MarcaRepuesto (Nombre, EsGenerica) Values (@nombre, @esGenerica);" +
                                         "SELECT CAST(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString, new { nombre, esGenerica });
                return new MarcaRepuesto(id, nombre, esGenerica);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
            
        }

        public static MarcaRepuesto GetMarca(int marcaRepuestoId)
        {
            const string sqlString = "Select * From MarcaRepuesto Where MarcaRepuesto_ID = @marcaRepuestoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<MarcaRepuesto>(sqlString, new {marcaRepuestoId});
        }

        public static List<MarcaRepuesto> GetMarcas()
        {
            const string sqlString = "Select * From MarcaRepuesto";
            return DbCliente.GetConexion().Query<MarcaRepuesto>(sqlString).ToList();
        }

        public override string ToString()
        {
            return $"Marca Repuesto: [ID: {MarcaRepuesto_ID} / Nombre: {Nombre} / Es Generica: {EsGenerica}]";
        }
        
    }
}