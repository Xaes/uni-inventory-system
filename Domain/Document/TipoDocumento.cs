using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Document
{
    public class TipoDocumento
    {

        public int TipoDocumento_ID { get; private set; }
        private int UltimoNumDoc;
        public string Nombre;
        public readonly bool CambiaUnidades, CambiaCosto;
        
        public TipoDocumento() {}
        
        private TipoDocumento(int tipoDocumentoId, int ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {
            this.TipoDocumento_ID = tipoDocumentoId;
            this.UltimoNumDoc = ultimoNumDoc;
            this.Nombre = nombre;
            this.CambiaUnidades = cambiaUnidades;
            this.CambiaCosto = cambiaCosto;
        }

        public static TipoDocumento AgregarTipo(int ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {

            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");
            
            try
            {
                const string sqlString =
                    "Insert Into TipoDocumento (UltimoNumDoc, Nombre, CambiaUnidades, CambiaCosto)" +
                    "Values (@ultimoNumDoc, @nombre, @cambiaUnidades, @cambiaCosto);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = (int) DbCliente.GetConexion().ExecuteScalar(
                    sqlString,
                    new {ultimoNumDoc, nombre, cambiaUnidades, cambiaCosto}
                );

                return new TipoDocumento(id, ultimoNumDoc, nombre, cambiaUnidades, cambiaCosto);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
            
        }
        
        public static TipoDocumento FindTipoDocumento(int tipoDocumentoId)
        {
            const string sqlString = "Select * From TipoDocumento Where TipoDocumento_ID = @tipoDocumentoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<TipoDocumento>(sqlString, new {tipoDocumentoId});
        }

        public static List<TipoDocumento> GetTipoDocumentos()
        {
            const string sqlString = "Select * From TipoDocumento";
            return DbCliente.GetConexion().Query<TipoDocumento>(sqlString).ToList();
        }

        public int GenerarCodigo()
        {
            return Convert.ToInt32($"{this.TipoDocumento_ID}{this.UltimoNumDoc}");
        }
        
        internal void ActualizarSecuencia()
        {
            this.UltimoNumDoc++;
            const string sqlString = "Update TipoDocumento Set UltimoNumDoc = @UltimoNumDoc Where TipoDocumento_ID = @TipoDocumento_ID";
            DbCliente.GetConexion().Execute(sqlString, new { this.UltimoNumDoc, this.TipoDocumento_ID });
        }

        public override string ToString()
        {
            return $"Tipo Documento: [ID: {TipoDocumento_ID} / Ultimo No. Doc: {UltimoNumDoc} / " +
                   $"Nombre: {Nombre} / Cambia Unidades: {CambiaUnidades} / Cambia Costo: {CambiaCosto}]";
        }
        
    }
}