using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Products
{
    public class Categoria
    {
        
        private int Categoria_ID { get; }
        private string Nombre, Descripcion;
        
        public Categoria() {}

        public Categoria(int categoriaId, string nombre, string descripcion)
        {
            this.Categoria_ID = categoriaId;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public static Categoria AgregarCategoria(string nombre, string descripcion)
        {
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");

            try
            {
                const string sqlString = "Insert Into Categoria (Nombre, Descripcion) Values (@nombre, @descripcion);" +
                                         "SELECT CAST(SCOPE_IDENTITY() as int)";
                
                var id = DbCliente.GetConexion().Execute(sqlString, new {nombre, descripcion});
                return new Categoria(id, nombre, descripcion);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
            
        }

        public static Categoria GetCategoria(int categoriaId)
        {
            const string sqlString = "Select * From Categoria Where Categoria_ID = @categoriaId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Categoria>(sqlString, new {categoriaId});
        }

        public static List<Categoria> GetCategorias()
        {
            const string sqlString = "Select * From Categoria";
            return DbCliente.GetConexion().Query<Categoria>(sqlString).ToList();
        }

        public List<SubCategoria> GetSubCategorias()
        {
            const string sqlString = "Select * From SubCategoria Where FK_Categoria_ID = @Categoria_ID";
            return DbCliente.GetConexion().Query<SubCategoria>(sqlString, new {this.Categoria_ID}).ToList();
        }

        public SubCategoria AgregarSubCategoria(string nombre, string descripcion)
        {
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");

            try
            {
                const string sqlString = "Insert Into SubCategoria (FK_Categoria_ID, Nombre, Descripcion) Values (@Categoria_ID, @nombre, @descripcion);" +
                                         "SELECT CAST(SCOPE_IDENTITY() as int)";
                
                var id = DbCliente.GetConexion().Execute(sqlString, new {this.Categoria_ID, nombre, descripcion});
                return new SubCategoria(id, Categoria_ID, nombre, descripcion);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
            
        }

        public override string ToString()
        {
            return $"Categoria: [ID: {Categoria_ID} / Nombre: {Nombre} / Descripcion: {Descripcion}]";
        }
        
    }
}