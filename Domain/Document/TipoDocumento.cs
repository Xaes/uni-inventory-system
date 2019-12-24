using System;

namespace Domain.Document
{
    public class TipoDocumento
    {

        private int TipoDocumento_ID;
        private string UltimoNumDoc, Nombre;
        private bool CambiaUnidades, CambiaCosto;
        
        public TipoDocumento() {}
        
        public TipoDocumento(int tipoDocumentoId, string ultimoNumDoc, string nombre, bool cambiaUnidades, bool cambiaCosto)
        {
            TipoDocumento_ID = tipoDocumentoId;
            UltimoNumDoc = ultimoNumDoc ?? throw new ArgumentNullException(nameof(ultimoNumDoc));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            CambiaUnidades = cambiaUnidades;
            CambiaCosto = cambiaCosto;
        }

    }
}