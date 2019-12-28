using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Domain.DB
{
    public class SqlExceptionMapper
    {

        public static Exception Map(SqlException ex)
        {
            Console.WriteLine("[INFO]: DB genero una Exception.");
            switch (ex.Number)
            {
                case 2627:
                    return new DuplicateNameException("[DB]: Datos de la entidad ya existen en la base de datos y deben ser unicos. Mensaje: " + ex.Message);
                case 2628:
                    return new ArgumentOutOfRangeException("", "[DB]: Datos proporcionados exceden el limite permitido. Mensaje: " + ex.Message);
                case 515:
                    return new ArgumentNullException("", "[DB]: Parametros de la entidad no pueden ser nulos. Mensaje: " + ex.Message);
                default:
                    Console.WriteLine(ex.Number);
                    throw ex;
            }
        }
        
    }
}