using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Providers
{
    public class Proveedor
    {

        public int Proveedor_ID { get; }
        public string Nombre { get; }

        public Proveedor(string nombre)
        {
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }

        public Proveedor(int proveedorId, string nombre)
        {
            this.Proveedor_ID = proveedorId;
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
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
            const string sqlString = "Insert Into Proveedor (Nombre) Values (@Nombre);" +
                                     "SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = DbCliente.GetConexion().Execute(sqlString, new {nombre});
            return new Proveedor(id, nombre);
        }

        public override string ToString()
        {
            return $"Proveedor: [ID: {Proveedor_ID} / Nombre: {Nombre}]";
        }
    }
}