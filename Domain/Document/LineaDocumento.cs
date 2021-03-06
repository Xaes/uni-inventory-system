using Dapper;
using Domain.DB;

namespace Domain.Document
{
    public class LineaDocumento
    {

        public int LineaDocumentoID, FK_DocumentoID, FK_MovimientoID, FK_RepuestoID;
        public int Unidades;
        public int? UnidadesNoRecibidas, UnidadesDanadas, CantidadPaquetes;
        public float? CostoUnitario, PrecioVentaUnitario;
        
        public LineaDocumento() {}
        
        public LineaDocumento(int lineaDocumentoId, int fkDocumentoId, int fkMovimientoId, int fkRepuestoId,
            int unidades, int? unidadesNoRecibidas, int? unidadesDanadas, int? cantidadPaquetes, 
            float? costoUnitario, float? precioVentaUnitario)
        {
            this.LineaDocumentoID = lineaDocumentoId;
            this.FK_DocumentoID = fkDocumentoId;
            this.FK_MovimientoID = fkMovimientoId;
            this.FK_RepuestoID = fkRepuestoId;
            this.Unidades = unidades;
            this.UnidadesNoRecibidas = unidadesNoRecibidas;
            this.UnidadesDanadas = unidadesDanadas;
            this.CantidadPaquetes = cantidadPaquetes;
            this.CostoUnitario = costoUnitario;
            this.PrecioVentaUnitario = precioVentaUnitario;
        }

        public Documento GetDocumento()
        {
            const string sqlString = "Select * From Documento Where Documento_ID = @FK_DocumentoID";
            return DbCliente.GetConexion().QueryFirstOrDefault<Documento>(sqlString, new {FK_DocumentoID});
        }

        public override string ToString()
        {
            return $"Linea Documento: [ID: {LineaDocumentoID} / Documento: {FK_DocumentoID} / " +
                   $"Repuesto: {FK_RepuestoID} / Movimiento: {FK_MovimientoID} / " +
                   $"Costo Unitario: {CostoUnitario} / Cantidad de Paquetes: {CantidadPaquetes} / " +
                   $"Precio Venta Unitario: {PrecioVentaUnitario}" + $"Unidades: {Unidades} / " +
                   $"Unidades Faltantes: {UnidadesNoRecibidas} / Unidades Danadas: {UnidadesDanadas}]";
        }


    }
}