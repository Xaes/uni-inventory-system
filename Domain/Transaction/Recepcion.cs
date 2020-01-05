using System;
using System.Collections.Generic;
using Domain.Document;
using Domain.Inventory;
using Domain.Locations;
using Domain.Products;
using Domain.Providers;

namespace Domain.Transaction
{
    public class RecepcionBuilder
    {
        
        private Recepcion recepcion = new Recepcion();

        public RecepcionBuilder() { this.Reset(); }

        public Recepcion Build()
        {
            
            // Estableciendo Tipo de Documento (Entrada por Compra).
            
            this.SetTipoDocumento(TipoDocumento.IDS[TipoDocumentos.ENTRADA_COMPRA]);
            
            // Verificar si los atributos no son nulos.
            
            if(this.recepcion.bodega == null || this.recepcion.fecha == DateTime.MinValue || this.recepcion.proveedor == null ||
               this.recepcion.tipoDocumento == null )
                throw new ArgumentNullException("", "Los parametros necesarios para construir una Recepcion no fueron proveidos.");
            
            var transaccion = this.recepcion;
            
            // Validar si hay productos.
            
            if(transaccion.repuestos.Count == 0)
                throw new InvalidOperationException("No se puede crear una recepcion sin haber agregado repuestos.");
            
            // Guardando las entidades correspondientes en la base de datos.
            // Agregando Documento.

            var documento = Documento.AgregarDocumento(
                transaccion.proveedor.Proveedor_ID,
                transaccion.tipoDocumento.TipoDocumento_ID,
                transaccion.fecha
            );
            
            // Agregando Lineas de Documento.
            
            transaccion.repuestos.ForEach((data) =>
            {
                
                // Generando Movimiento para cada Producto.
                
                Movimiento movimiento = Movimiento.AgregarMovimiento(
                    null,
                    data["localizacion"],
                    Periodo.GetPeriodoActivo().Periodo_ID,
                    data["costoUnitario"],
                    null,
                    data["unidades"],
                    transaccion.fecha,
                    TipoTransaccion.ENTRADA
                );
                
                // Generando Linea de Documento.

                documento.AgregarLinea(
                    movimiento.Movimiento_ID,
                    data["repuesto"],
                    transaccion.bodega.Bodega_ID,
                    data["unidades"],
                    data["unidadesFaltantes"],
                    data["unidadesDanadas"],
                    data["cantidadPaquetes"],
                    data["costoUnitario"],
                    null
                );
                
                // Generando Costo.

                Costo.AgregarCosto(
                    data["repuesto"],
                    data["unidades"],
                    transaccion.fecha,
                    data["costoUnitario"]
                );
                
                // Generando Existencia.

                Existencia.AgregarExistencia(
                    data["repuesto"],
                    data["unidades"],
                    data["localizacion"]
                );

            });

            this.Reset();
            return transaccion;
            
        }

        public void AgregarProductos(int productoId, int unidades, int unidadedFaltantes,
            int unidadesDanadas, int cantidadPaquetes, float costoUnitario, Localizacion localizacion)
        {
            
            // Validar si existe una Bodega ligada a Recepcion primero.

            if (this.recepcion.bodega == null)
                throw new InvalidOperationException("Una bodega debe agregarse a la Recepcion primero.");

            // Validar si algun parametro esta fuera del minimo aceptable.

            if (unidades <= 0 || unidadedFaltantes < 0 || unidadesDanadas < 0 || cantidadPaquetes < 0)
                throw new ArgumentOutOfRangeException("", "Las Unidades deberian ser mayores a 0. " + 
                    "Las Unidades Faltantes, Danadas y Cantidad de Paquetes deben ser mayores o igual que 0.");
            
            // Traer / Validar si el producto existe.
            
            var repuesto = Repuesto.FindRepuesto(productoId);
            
            if(repuesto == null)
                throw new ArgumentException("El producto no existe en la base de datos.");
            
            // Validar que la Bodega sea la misma que la Localizacion.

            if (!this.recepcion.bodega.Codigo.Equals(localizacion.GetBodega().Codigo))
                throw new ArgumentException("La localizacion debe pertenecer a la misma bodega indicada");
            
            // Agregando el Producto del Documento a la lista.
            
            this.recepcion.repuestos.Add(new Dictionary<string, dynamic>()
            {
                { "repuesto", repuesto.Repuesto_ID },
                { "localizacion", localizacion.Codigo },
                { "unidades", unidades },
                { "unidadesFaltantes", unidadedFaltantes },
                { "unidadesDanadas", unidadesDanadas },
                { "costoUnitario", costoUnitario },
                { "cantidadPaquetes", cantidadPaquetes }
            });

        }

        public void SetBodega(int bodegaId)
        {
            this.recepcion.bodega = Bodega.FindBodega(bodegaId);
        }
        
        private void SetTipoDocumento(int tipoDocumentoId)
        {
            this.recepcion.tipoDocumento = TipoDocumento.FindTipoDocumento(tipoDocumentoId);
        }
        
        public void SetProveedor(int proveedorId)
        {
            this.recepcion.proveedor = Proveedor.FindProveedor(proveedorId);
        }

        public void SetFecha(DateTime fecha)
        {
            Periodo.DentroPeriodoActivo(fecha);
            this.recepcion.fecha = fecha;
        }

        public void Reset()
        {
            this.recepcion = new Recepcion();
        }
        
    }

    public class Recepcion
    {
        
        internal TipoDocumento tipoDocumento;
        internal Bodega bodega;
        internal Proveedor proveedor;
        internal DateTime fecha;
        internal List<Dictionary<string, dynamic>> repuestos { get; }

        internal Recepcion()
        {
            this.repuestos = new List<Dictionary<string, dynamic>>();
        }

    }
    
}