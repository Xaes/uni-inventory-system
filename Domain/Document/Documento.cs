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
        public int FK_Bodega_ID;
        public int? FK_Proveedor_ID;
        public DateTime Fecha;
        
        public Documento() {}
        
        private Documento(int documentoId, int fkBodegaId, int? fkProveedorId, int tipoDocumentoId, int numeroDoc, DateTime fecha)
        {
            this.Documento_ID = documentoId;
            this.FK_Proveedor_ID = fkProveedorId;
            this.FK_Bodega_ID = fkBodegaId;
            this.TipoDocumento_ID = tipoDocumentoId;
            this.NumeroDoc = numeroDoc;
            this.Fecha = fecha;
        }

        public static Documento AgregarDocumento(int fkBodegaId, int? fkProveedorId, int tipoDocumentoId, DateTime fecha)
        {
            try
            {
                var tipoDocumento = TipoDocumento.FindTipoDocumento(tipoDocumentoId);
                var numeroDoc = tipoDocumento.GenerarCodigo();
                
                const string sqlString =
                    "Insert Into Documento (FK_BodegaID, FK_ProveedorID, FK_TipoDocumentoID, NumeroDoc, Fecha)" +
                    "Values (@fkBodegaId, @fkProveedorId, @tipoDocumentoId, @numeroDoc, @fecha);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = (int) DbCliente.GetConexion().ExecuteScalar(
                    sqlString,
                    new {fkBodegaId, fkProveedorId, tipoDocumentoId, numeroDoc, fecha}
                );

                var documento = new Documento(id, fkBodegaId, fkProveedorId, tipoDocumentoId, numeroDoc, fecha);
                tipoDocumento.ActualizarSecuencia();
                return documento;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }
        
        public LineaDocumento AgregarLinea(int fkMovimientoId, int fkRepuestoId,
            int unidades, int? unidadesNoRecibidas, int? unidadesDanadas, int? cantidadPaquetes,
            float? costoUnitario, float? precioVentaUnitario)
        {
            
            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0 || unidadesNoRecibidas < 0 || unidadesDanadas < 0 || cantidadPaquetes < 0)
                throw new ArgumentOutOfRangeException("", "Las Unidades deberian ser mayores a 0. " + 
                    "Las Unidades Faltantes, Danadas y Cantidad de Paquetes deben ser mayores o igual que 0.");
            
            try
            {
                const string sqlString = 
                    "Insert Into LineaDocumento (FK_DocumentoID, FK_MovimientoID, FK_RepuestoID, " +
                    "Unidades, UnidadesNoRecibidas, UnidadesDanadas, CantidadPaquetes, CostoUnitario, PrecioVentaUnitario)" +
                    "Values (@Documento_ID, @fkMovimientoId, @fkRepuestoId, @unidades, @unidadesNoRecibidas, " +
                    "@unidadesDanadas, @cantidadPaquetes, @costoUnitario, @precioVentaUnitario);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString, new
                {
                    this.Documento_ID, fkMovimientoId, fkRepuestoId, unidades, unidadesNoRecibidas, unidadesDanadas,
                    cantidadPaquetes, costoUnitario, precioVentaUnitario
                });
                
                return new LineaDocumento(id, this.Documento_ID, fkMovimientoId, fkRepuestoId,
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
            return $"Documento: [ID: {Documento_ID} / Proveedor: {FK_Proveedor_ID} / " +
                   $"Tipo Documento: {TipoDocumento_ID} / No. Doc: {NumeroDoc} / Fecha: {Fecha}]";
        }
        

    }
}