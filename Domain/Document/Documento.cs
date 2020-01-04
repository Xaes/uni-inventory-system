using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Document
{
    public class Documento
    {

        public int Documento_ID { get; }
        public int TipoDocumento_ID, NumeroDoc;
        public int? Proveedor_ID;
        public DateTime Fecha;
        
        public Documento() {}
        
        private Documento(int documentoId, int? proveedorId, int tipoDocumentoId, int numeroDoc, DateTime fecha)
        {
            this.Documento_ID = documentoId;
            this.Proveedor_ID = proveedorId;
            this.TipoDocumento_ID = tipoDocumentoId;
            this.NumeroDoc = numeroDoc;
            this.Fecha = fecha;
        }

        public static Documento AgregarDocumento(int? proveedorId, int tipoDocumentoId, DateTime fecha)
        {
            try
            {
                var tipoDocumento = TipoDocumento.FindTipoDocumento(tipoDocumentoId);
                var numeroDoc = tipoDocumento.GenerarCodigo();
                
                const string sqlString =
                    "Insert Into Documento (FK_ProveedorID, FK_TipoDocumentoID, NumeroDoc, Fecha)" +
                    "Values (@proveedorId, @tipoDocumentoId, @numeroDoc, @fecha);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = (int) DbCliente.GetConexion().ExecuteScalar(
                    sqlString,
                    new {proveedorId, tipoDocumentoId, numeroDoc, fecha}
                );

                var documento = new Documento(id, proveedorId, tipoDocumentoId, numeroDoc, fecha);
                tipoDocumento.ActualizarSecuencia();
                return documento;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }
        
        public LineaDocumento AgregarLinea(int fkMovimientoId, int fkRepuestoId,
            int fkBodegaId, int unidades, int? unidadesNoRecibidas, int? unidadesDanadas, int? cantidadPaquetes,
            float? costoUnitario, float? precioVentaUnitario)
        {
            try
            {
                const string sqlString = 
                    "Insert Into LineaDocumento (FK_DocumentoID, FK_MovimientoID, FK_RepuestoID, FK_BodegaID, " +
                    "Unidades, UnidadesNoRecibidas, UnidadesDanadas, CantidadPaquetes, CostoUnitario, PrecioVentaUnitario)" +
                    "Values (@Documento_ID, @fkMovimientoId, @fkRepuestoId, @fkBodegaId, @unidades, @unidadesNoRecibidas, " +
                    "@unidadesDanadas, @cantidadPaquetes, @costoUnitario, @precioVentaUnitario);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString, new
                {
                    this.Documento_ID, fkMovimientoId, fkRepuestoId, fkBodegaId, unidades, unidadesNoRecibidas, unidadesDanadas,
                    cantidadPaquetes, costoUnitario, precioVentaUnitario
                });
                
                return new LineaDocumento(id, this.Documento_ID, fkMovimientoId, fkRepuestoId, fkBodegaId,
                    unidades, unidadesNoRecibidas, unidadesDanadas, cantidadPaquetes, costoUnitario, precioVentaUnitario);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }
        
        public static Documento FindDocumento(int documentoId)
        {
            const string sqlString = "Select * From Documento Where Documento_ID = @documentoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<Documento>(sqlString, new {documentoId});
        }

        public static List<Documento> GetDocumentos()
        {
            const string sqlString = "Select * From Documento";
            return DbCliente.GetConexion().Query<Documento>(sqlString).ToList();
        }

        public List<LineaDocumento> GetLineas()
        {
            const string sqlString = "Select * From LineaDocumento Where FK_DocumentoID = @Documento_ID";
            return DbCliente.GetConexion().Query<LineaDocumento>(sqlString, new {Documento_ID}).ToList();
        }

        public override string ToString()
        {
            return $"Documento: [ID: {Documento_ID} / Proveedor: {Proveedor_ID} / " +
                   $"Tipo Documento: {TipoDocumento_ID} / No. Doc: {NumeroDoc} / Fecha: {Fecha}]";
        }
        

    }
}