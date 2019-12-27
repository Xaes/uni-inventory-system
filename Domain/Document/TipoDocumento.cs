using System;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Document
{
    public class TipoDocumento
    {

        private int TipoDocumento_ID;
        private string UltimoNumDoc, Nombre;
        private readonly bool CambiaUnidades, CambiaCosto;
        
        public TipoDocumento() {}
        
        private TipoDocumento(int tipoDocumentoId, string ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {
            this.TipoDocumento_ID = tipoDocumentoId;
            this.UltimoNumDoc = ultimoNumDoc;
            this.Nombre = nombre;
            this.CambiaUnidades = cambiaUnidades;
            this.CambiaCosto = cambiaCosto;
        }

        public TipoDocumento AgregarTipo(string ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {
            
            if(string.IsNullOrWhiteSpace(ultimoNumDoc))
                throw new ArgumentNullException(nameof(ultimoNumDoc), "UltimoNumDoc no puede ser nulo, estar vacio o solo contener espacios.");
            
            if(string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentNullException(nameof(nombre), "Nombre no puede ser nulo, estar vacio o solo contener espacios.");
            
            try
            {
                const string sqlString =
                    "Insert Into TipoDocumento (UltimoNumDoc, Nombre, CambiaUnidades, CambiaCost)" +
                    "Values (@ultimoNumDoc, @nombre, @cambiaUnidades, @cambiaCosto);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = (int) DbCliente.GetConexion().ExecuteScalar(
                    sqlString,
                    new {ultimoNumDoc, nombre, CambiaUnidades, CambiaCosto}
                );

                return new TipoDocumento(id, ultimoNumDoc, nombre, cambiaUnidades, cambiaCosto);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
            
        }
    }
}