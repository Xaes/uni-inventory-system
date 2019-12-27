using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Providers
{
    public class Proveedor
    {

        public int Proveedor_ID { get; }
        public string Nombre { get; }

        public Proveedor() {}

        public Proveedor(int proveedorId, string nombre)
        {
            this.Proveedor_ID = proveedorId;
            this.Nombre = nombre;
        }
        
        public static List<Proveedor> GetProveedores()
        {
            const string sqlString = "Select * from Proveedor";
            return DbCliente.GetConexion().Query<Proveedor>(sqlString).ToList();
        }

        public static Proveedor GetProveedor(int id)
        {
            const string sqlString = "Select * from Proveedor Where Proveedor_ID = @id";
            return DbCliente.GetConexion().QueryFirst<Proveedor>(sqlString, new {id});
        }

        public static Proveedor AgregarProveedor(string nombre)
        {
            try {
                
                if(string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");
                
                const string sqlString = "Insert Into Proveedor (Nombre) Values (@Nombre);" +
                                         "SELECT CAST(SCOPE_IDENTITY() as int)";
                
                var id = DbCliente.GetConexion().Execute(sqlString, new {nombre});
                return new Proveedor(id, nombre);
            } catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public override string ToString()
        {
            return $"Proveedor: [ID: {Proveedor_ID} / Nombre: {Nombre}]";
        }
    }
}