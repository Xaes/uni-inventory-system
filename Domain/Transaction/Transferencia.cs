using System;
using System.Collections.Generic;
using Domain.Document;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;

namespace Domain.Transaction
{
    public class TransferenciaBuilder
    {

        private Transferencia transferencia = new Transferencia();

        public TransferenciaBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.transferencia = new Transferencia();
        }

        public Transferencia Build()
        {
            
            // Definiendo el Tipo de Documento.
            
            this.SetTipoDocumento(TipoDocumento.FindTipoDocumento(TipoDocumento.IDS[TipoDocumentos.TRANSFERENCIA]).TipoDocumento_ID);
            
            // Validar si los atributos no son nulos.
            
            if(this.transferencia.bodegaDestino == null || this.transferencia.bodegaOrigen == null ||
               this.transferencia.tipoDocumento == null || this.transferencia.fecha == DateTime.MinValue )
                throw new ArgumentNullException("", "Los parametros necesarios para construir una Transferencia no fueron proveidos.");
            
            var transaccion = this.transferencia;
            
            // Validar si hay productos.
            
            if(transaccion.repuestos.Count == 0)
                throw new InvalidOperationException("No se puede crear una recepcion sin haber agregado repuestos.");
            
            // Guardando las entidades correspondientes en la base de datos.
            // Agregando Documento.

            var documento = Documento.AgregarDocumento(
                transaccion.bodegaOrigen.Bodega_ID,
                null,
                transaccion.tipoDocumento.TipoDocumento_ID,
                transaccion.fecha
            );
            
            // Agregando Lineas de Documento.

            transaccion.repuestos.ForEach((data) =>
            {
                
                // Generando Movimiento para ambas Bodegas.

                Movimiento.AgregarMovimiento(
                    data["localizacionOrigen"],
                    data["localizacionDestino"],
                    Periodo.GetPeriodoActivo().Periodo_ID,
                    null,
                    null,
                    data["unidades"],
                    transaccion.fecha,
                    TipoTransaccion.SALIDA
                );
                
                Movimiento.AgregarMovimiento(
                    data["localizacionOrigen"],
                    data["localizacionDestino"],
                    Periodo.GetPeriodoActivo().Periodo_ID,
                    null,
                    null,
                    data["unidades"],
                    transaccion.fecha,
                    TipoTransaccion.SALIDA
                );
                
                // Extrayendo Existencias.

                Existencia existenciaOrigen = Existencia.FindExistencia(data["existenciaRepuesto"]);
                existenciaOrigen.ExtraerUnidades(data["unidades"]);
                
                // Creando nueva Existencia

                Existencia.AgregarExistencia(
                existenciaOrigen.Existencia_ID,
                data["unidades"],
                data["localizacionDestino"]
                );
                
            });
            
            this.Reset();
            return transaccion;
        }

        public void SetBodegaOrigen(int bodegaId)
        {
            this.transferencia.bodegaOrigen = Bodega.FindBodega(bodegaId);
        }

        public void SetBodegaDestino(int bodegaId)
        {
            this.transferencia.bodegaDestino = Bodega.FindBodega(bodegaId);
        }

        private void SetTipoDocumento(int tipoDocumentoId)
        {
            this.transferencia.tipoDocumento = TipoDocumento.FindTipoDocumento(tipoDocumentoId);
        }

        public void SetFecha(DateTime fecha)
        {
            Periodo.DentroPeriodoActivo(fecha);
            this.transferencia.fecha = fecha;
        }

        public void AgregarProductos(int existenciaId, string localizacionCodigo, int unidades)
        {
            
            // Validar si existe una Bodega ligada a Recepcion primero.

            if (this.transferencia.bodegaDestino == null && this.transferencia.bodegaOrigen == null)
                throw new InvalidOperationException("Una bodega de origen y otra de destino deben agregarse a la transferencia.");

            // Validar si la localizacion pertenece a la Bodega de Destino.
            
            if(Localizacion.GetLocalizacion(localizacionCodigo).GetBodega().Bodega_ID != this.transferencia.bodegaDestino.Bodega_ID)
                throw new InvalidOperationException("La localizacion no pertenece a la misma Bodega de Destino");

            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0)
                throw new ArgumentOutOfRangeException(nameof(unidades), "Las Unidades deberian ser mayores a 0.");
            
            // Traer / Validar si el producto tiene existencias en la base de datos.

            var existencia = Existencia.FindExistencia(existenciaId);
            
            if(existencia == null)
                throw new ArgumentException("El producto no tiene existencias en la base de datos.");
            
            // Validar si la Existencia Pertenece a la Bodega de Origen.
            
            if(Localizacion.GetLocalizacion(existencia.FK_Localizacion_ID).GetBodega().Bodega_ID 
               != this.transferencia.bodegaOrigen.Bodega_ID)
                throw new InvalidOperationException("La existencia proporcionada no pertenece a la misma Bodega de Origen.");
            
            // Guardando Repuestos en la Lista.
            
            this.transferencia.repuestos.Add(new Dictionary<string, dynamic>()
            {
                {"existenciaRepuesto", existenciaId},
                {"localizacionOrigen", existencia.FK_Localizacion_ID},
                {"localizacionDestino", localizacionCodigo},
                {"unidades", unidades}
            });

        }

    }

    public class Transferencia
    {

        internal Bodega bodegaOrigen, bodegaDestino;
        internal TipoDocumento tipoDocumento;
        internal DateTime fecha;
        internal List<Dictionary<string, dynamic>> repuestos { get; }
        
        internal Transferencia()
        {
            this.repuestos = new List<Dictionary<string, dynamic>>();
        }
        
    }
}