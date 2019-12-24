using System;
using Dapper;
using Domain.DB;

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
            this.UltimoNumDoc = ultimoNumDoc ?? throw new ArgumentNullException(nameof(ultimoNumDoc));
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.CambiaUnidades = cambiaUnidades;
            this.CambiaCosto = cambiaCosto;
        }

        public TipoDocumento AgregarTipo(string ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {
            const string sqlString = "Insert Into TipoDocumento (UltimoNumDoc, Nombre, CambiaUnidades, CambiaCost)" +
                                     "Values (@ultimoNumDoc, @nombre, @cambiaUnidades, @cambiaCosto);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(
                sqlString, 
                new { ultimoNumDoc, nombre, CambiaUnidades, CambiaCosto }
            );
            
            return new TipoDocumento(id, ultimoNumDoc, nombre, cambiaUnidades, cambiaCosto);
        }

    }
}