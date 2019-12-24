using System;
using Dapper;
using Domain.DB;

namespace Domain.Document
{
    public class Documento
    {

        private int Documento_ID;
        private int Proveedor_ID;
        private int TipoDocumento_ID;
        private int NumeroDoc;
        private DateTime Fecha;
        
        private Documento(int documentoId, int proveedorId, int tipoDocumentoId, int numeroDoc, DateTime fecha)
        {
            this.Documento_ID = documentoId;
            this.Proveedor_ID = proveedorId;
            this.TipoDocumento_ID = tipoDocumentoId;
            this.NumeroDoc = numeroDoc;
            this.Fecha = fecha;
        }

        public static Documento AgregarDocumento(int proveedorId, int tipoDocumentoId, int numeroDoc, DateTime fecha)
        {
            const string sqlString = "Insert Into Documento (FK_ProveedorID, FK_TipoDocumentoID, NumeroDoc, Fecha)" +
                                     "Values (@proveedorId, @tipoDocumentoId, @numeroDoc, @fecha);" +
                                     "Select Cast(SCOPE_IDENTITY() as int)";

            var id = (int) DbCliente.GetConexion().ExecuteScalar(
                sqlString, 
                new { proveedorId, tipoDocumentoId, numeroDoc, fecha }
            );
            
            return new Documento(id, proveedorId, tipoDocumentoId, numeroDoc, fecha);
        }

    }
}