using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Document;
using Domain.Locations;
using Domain.Providers;

namespace Domain.Transaction
{
    
    public class RecepcionBuilder
    {
        
        private Recepcion recepcion = new Recepcion();
        
        public RecepcionBuilder() { this.Reset(); }

        public Recepcion Build()
        {
            var resultado = this.recepcion;
            this.Reset();
            return resultado;
        }
        
        public void Reset()
        {
            this.recepcion = new Recepcion();
        }
        
    }

    public class Recepcion
    {
        
        private TipoDocumento tipoDocumento;
        private Bodega bodega;
        private Proveedor proveedor;
        private DateTime fecha;
        private Documento documento;
        private List<LineaDocumento> lineasDocumento;
        
    }
}