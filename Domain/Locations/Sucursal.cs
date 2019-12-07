using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class Sucursal
    {
        public static List<Bodega> GetBodegas()
        {
            return DbCliente.GetConexion()
                .Query<Bodega>("Select * From Bodega")
                .ToList();
        }

        public static Bodega FindBodega(int id)
        {
            return DbCliente.GetConexion()
                .Query<Bodega>("Select * From Bodega Where Bodega_ID = @id", new { id })
                .SingleOrDefault();
        }

    }
}