using System;

namespace Domain.Inventory
{
    public class Movimiento
    {
        
        private int Movimiento_ID;
        private int LocalizacionInicial_ID;
        private int LocalizacionFinal_ID;
        private float CostoTotal, CostoUnitario, PrecioVentaUnitario;
        private int Unidades;
        private DateTime Fecha;
        private string TipoTransaccion;

        public Movimiento() {}
        
        public Movimiento(int movimientoId, int localizacionInicialId, int localizacionFinalId,
            float costoTotal, float costoUnitario, float precioVentaUnitario,
            int unidades, DateTime fecha, string tipoTransaccion
        )
        {
            this.Movimiento_ID = movimientoId;
            this.LocalizacionInicial_ID = localizacionInicialId;
            this.LocalizacionFinal_ID = localizacionFinalId;
            this.CostoTotal = costoTotal;
            this.CostoUnitario = costoUnitario;
            this.PrecioVentaUnitario = precioVentaUnitario;
            this.Unidades = unidades;
            this.Fecha = fecha;
            this.TipoTransaccion = tipoTransaccion;
        }
        
        public Movimiento(int localizacionInicialId, int localizacionFinalId,
            float costoTotal, float costoUnitario, float precioVentaUnitario,
            int unidades, DateTime fecha, string tipoTransaccion
        )
        {
            this.LocalizacionInicial_ID = localizacionInicialId;
            this.LocalizacionFinal_ID = localizacionFinalId;
            this.CostoTotal = costoTotal;
            this.CostoUnitario = costoUnitario;
            this.PrecioVentaUnitario = precioVentaUnitario;
            this.Unidades = unidades;
            this.Fecha = fecha;
            this.TipoTransaccion = tipoTransaccion;
        }
        
    }
}