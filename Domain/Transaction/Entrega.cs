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

            // Estableciendo Tipo de Documento (Salida por Venta).

            this.SetTipoDocumento(TipoDocumento.IDS[TipoDocumentos.SALIDA_VENTA]);
            
            // Verificar si los atributos no son nulos.
            
            if(this.entrega.bodega == null || this.entrega.fecha == DateTime.MinValue || this.entrega.tipoDocumento == null)
                throw new ArgumentNullException("", "Los parametros necesarios para construir una Recepcion no fueron proveidos.");
            
            var transaccion = this.entrega;
            
            // Validar si hay existencias.
            
            if(transaccion.existencias.Count == 0)
                throw new InvalidOperationException("No se puede crear una entrega sin haber agregado existencias.");
            
            // Guardando las entidades correspondientes en la base de datos.
            // Agregando Documento.

            var documento = Documento.AgregarDocumento(
                transaccion.bodega.Bodega_ID,
                transaccion.proveedor?.Proveedor_ID,
                transaccion.tipoDocumento.TipoDocumento_ID,
                transaccion.fecha
            );
            
            // Agregando Lineas de Documento y Movimientos.

            transaccion.existencias.ForEach((data) =>
            {
                
                // Extrayendo y Actualizando Costos.

                List<Dictionary<string, dynamic>> proyeccion = Costo.ExtraerCosto(
                    data["repuesto"],
                    data["unidadesAExtraer"]
                );
                
                // Generando Movimientos y Lineas de Documento por cada distinto costo.

                proyeccion.ForEach((p) =>
                {
                    
                    Movimiento movimiento = Movimiento.AgregarMovimiento(
                        data["localizacion"],
                        null,
                        Periodo.GetPeriodoActivo().Periodo_ID,
                        p["costoUnitario"],
                        data["precioVenta"],
                        p["cantidad"],
                        transaccion.fecha,
                        TipoTransaccion.SALIDA
                    );

                    var m = Movimiento.GetMovimientos();
                    
                    // Generando Linea de Documento.

                    documento.AgregarLinea(
                        movimiento.Movimiento_ID,
                        data["repuesto"],
                        p["cantidad"],
                        null,
                        null,
                        null,
                        p["costoUnitario"],
                        data["precioVenta"]
                    );
                    
                });
                
                // Actualizando Existencias.

                Existencia.FindExistencia((int) data["existencia"]).ExtraerUnidades(data["unidadesAExtraer"]);

            });

            this.Reset();
            return transaccion;
        }

        public void AgregarProductos(int existenciaId, int unidades, float precioVenta)
        {
            
            // Validar si existe una Bodega ligada a Recepcion primero.

            if (this.entrega.bodega == null)
                throw new InvalidOperationException("Una bodega debe agregarse a la Recepcion primero.");

            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0 || precioVenta < 0)
                throw new ArgumentOutOfRangeException(nameof(unidades), "Las Unidades deberian ser mayores o iguales a 0." +
                                                                        "El precio de venta debe ser mayor a 0.");
            
            // Traer / Validar si la existencia es valida.
            
            var existencia = Existencia.FindExistencia(existenciaId);
            
            if(existencia == null)
                throw new ArgumentException("La existencia no esta presente en la base de datos.");
            
            // Validar que la Bodega sea la misma que la Localizacion.

            if (!this.entrega.bodega.Codigo.Equals(Localizacion.GetLocalizacion(existencia.FK_Localizacion_ID).GetBodega().Codigo))
                throw new ArgumentException("La localizacion de la existencia debe pertenecer a la misma bodega indicada.");
            
            // Validar que las unidades no sean mas que las disponibles en existencia.
            
            if(unidades > existencia.Unidades)
                throw new ArgumentException("Las unidades a extraer no pueden ser mayores que las disponibles.");
            
            // Agregando el Producto del Documento a la lista.
            
            this.entrega.existencias.Add(new Dictionary<string, dynamic>()
            {
                { "existencia", existencia.Existencia_ID },
                { "repuesto", existencia.FK_Repuesto_ID },
                { "localizacion", existencia.FK_Localizacion_ID },
                { "unidadesAExtraer", unidades },
                { "precioVenta", precioVenta }
            });
            
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
        
        internal List<Dictionary<string, dynamic>> existencias { get; }

        internal Entrega()
        {
            this.existencias = new List<Dictionary<string, dynamic>>();
        }
    }
}