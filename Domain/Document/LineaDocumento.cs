using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Document
{
    public class LineaDocumento
    {

        public int LineaDocumentoID, FK_DocumentoID, FK_MovimientoID, FK_RepuestoID, FK_BodegaID;
        public int Unidades;
        public int? UnidadesNoRecibidas, UnidadesDanadas, CantidadPaquetes;
        public float? CostoUnitario, PrecioVentaUnitario;
        
        public LineaDocumento() {}
        
        public LineaDocumento(int lineaDocumentoId, int fkDocumentoId, int fkMovimientoId, int fkRepuestoId, 
            int fkBodegaId, int unidades, int? unidadesNoRecibidas, int? unidadesDanadas, int? cantidadPaquetes, 
            float? costoUnitario, float? precioVentaUnitario)
        {
            this.LineaDocumentoID = lineaDocumentoId;
            this.FK_DocumentoID = fkDocumentoId;
            this.FK_MovimientoID = fkMovimientoId;
            this.FK_RepuestoID = fkRepuestoId;
            this.FK_BodegaID = fkBodegaId;
            this.Unidades = unidades;
            this.UnidadesNoRecibidas = unidadesNoRecibidas;
            this.UnidadesDanadas = unidadesDanadas;
            this.CantidadPaquetes = cantidadPaquetes;
            this.CostoUnitario = costoUnitario;
            this.PrecioVentaUnitario = precioVentaUnitario;
        }

        public static LineaDocumento AgregarLinea(int fkDocumentoId, int fkMovimientoId, int fkRepuestoId,
            int fkBodegaId, int unidades, int? unidadesNoRecibidas, int? unidadesDanadas, int? cantidadPaquetes,
            float? costoUnitario, float? precioVentaUnitario)
        {
            try
            {
                const string sqlString = 
                    "Insert Into LineaDocumento (FK_DocumentoID, FK_MovimientoID, FK_RepuestoID, FK_BodegaID, " +
                    "Unidades, UnidadesNoRecibidas, UnidadesDanadas, CantidadPaquetes, CostoUnitario, PrecioVentaUnitario)" +
                    "Values (@fkDocumentoId, @fkMovimientoId, @fkRepuestoId, @fkBodegaId, @unidades, @unidadesNoRecibidas, " +
                    "@unidadesDanadas, @cantidadPaquetes, @costoUnitario, @precioVentaUnitario);" +
                    "Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString, new
                {
                    fkDocumentoId, fkMovimientoId, fkRepuestoId, fkBodegaId, unidades, unidadesNoRecibidas, unidadesDanadas,
                    cantidadPaquetes, costoUnitario, precioVentaUnitario
                });
                
                return new LineaDocumento(id, fkDocumentoId, fkMovimientoId, fkRepuestoId, fkBodegaId, unidades, 
                    unidadesNoRecibidas, unidadesDanadas, cantidadPaquetes, costoUnitario, precioVentaUnitario);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public Documento GetDocumento()
        {
            const string sqlString = "Select * From Documento Where Documento_ID = @FK_DocumentoID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Documento>(sqlString, new {FK_DocumentoID});
        }

        public override string ToString()
        {
            return $"Linea Documento: [ID: {LineaDocumentoID} / Documento: {FK_DocumentoID} / " +
                   $"Repuesto: {FK_RepuestoID} / Movimiento: {FK_MovimientoID} / Bodega: {FK_BodegaID} / " +
                   $"Costo Unitario: {CostoUnitario} / Cantidad de Paquetes: {CantidadPaquetes} / " +
                   $"Precio Venta Unitario: {PrecioVentaUnitario}" + $"Unidades: {Unidades} / " +
                   $"Unidades Faltantes: {UnidadesNoRecibidas} / Unidades Danadas: {UnidadesDanadas}]";
        }


    }
}