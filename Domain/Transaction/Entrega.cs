#nullable enable
using System;
using System.Collections.Generic;
using Domain.Document;
using Domain.Inventory;
using Domain.Locations;
using Domain.Providers;

namespace Domain.Transaction
{
    public class EntregaBuilder
    {
        
        private Entrega entrega = new Entrega();
        
        public Entrega Build()
        {

            // Estableciendo Tipo de Documento (Entrada por Compra).

            this.SetTipoDocumento(TipoDocumento.IDS[TipoDocumentos.SALIDA_COMPRA]);

            return this.entrega;
        }
        
        public void SetBodega(int bodegaId)
        {
            this.entrega.bodega = Bodega.FindBodega(bodegaId);
        }
        
        private void SetTipoDocumento(int tipoDocumentoId)
        {
            this.entrega.tipoDocumento = TipoDocumento.FindTipoDocumento(tipoDocumentoId);
        }
        
        public void SetProveedor(int proveedorId)
        {
            this.entrega.proveedor = Proveedor.FindProveedor(proveedorId);
        }

        public void SetFecha(DateTime fecha)
        {
            Periodo.DentroPeriodoActivo(fecha);
            this.entrega.fecha = fecha;
        }

        public void Reset()
        {
            this.entrega = new Entrega();
        }
    }
    
    public class Entrega
    {
        internal TipoDocumento tipoDocumento;
        internal Bodega bodega;
        internal Proveedor? proveedor;
        internal DateTime fecha;
        
        internal List<Dictionary<string, dynamic>> repuestos { get; }

        internal Entrega()
        {
            this.repuestos = new List<Dictionary<string, dynamic>>();
        }
    }
}