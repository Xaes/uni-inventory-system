using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Bodega
    {
        
        public int Bodega_ID { get; }
        private string Nombre { get; }
        public string Codigo { get; }
        
        public Bodega() {}

        private Bodega(int bodegaId, string codigo, string nombre)
        {
            this.Bodega_ID = bodegaId;
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
        }

        public static List<Bodega> GetBodegas()
        {
            const string sqlString = "Select * From Bodega";
            return DbCliente.GetConexion().Query<Bodega>(sqlString).ToList();
        }
        
        public static Bodega FindBodega(int id)
        {
            const string sqlString = "Select * From Bodega Where Bodega_ID = @id";
            return DbCliente.GetConexion().QueryFirst<Bodega>(sqlString, new {id});
        }
        
        public List<Pasillo> GetPasillos()
        {
            const string sqlString = "Select * From Pasillo Where FK_Bodega_ID = @Bodega_ID";
            return DbCliente.GetConexion().Query<Pasillo>(sqlString, new {this.Bodega_ID}).ToList();
        }
        
        public Pasillo AgregarPasillo(string codigo)
        {
            var pasillo = new Pasillo(codigo, this.Bodega_ID);
            pasillo.Insertar();
            return pasillo;
        }

        public static Bodega AgregarBodega(string nombre, string codigo)
        {
            const string sqlString = "Insert Into Bodega (Codigo, Nombre) Values (@Codigo, @Nombre);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(sqlString, new { nombre, codigo });
            return new Bodega(id, codigo, nombre);
        }

        public override string ToString()
        {
            return $"Bodega: [Codigo: {Codigo} / Nombre: {Nombre} / ID: {Bodega_ID}]";
        }
    }
}