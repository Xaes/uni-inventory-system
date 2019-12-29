using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;
using Microsoft.Data.SqlClient;

namespace Domain.Products
{
    public class ModeloVehiculo
    {

        public int ModeloVehiculo_ID;
        public string NombreModelo, Marca, Version;
        
        public ModeloVehiculo() {}
        
        private ModeloVehiculo(int modeloVehiculoId, string nombreModelo, string marca, string version)
        {
            this.ModeloVehiculo_ID = modeloVehiculoId;
            this.NombreModelo = nombreModelo;
            this.Marca = marca;
            this.Version = version;
        }

        public static ModeloVehiculo AgregarModelo(string nombreModelo, string marca, string version)
        {
            
            if (string.IsNullOrWhiteSpace(nombreModelo))
                throw new ArgumentNullException(nameof(nombreModelo),"NombreModelo no puede ser null, estar vacias o solo contener espacios.");
            
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentNullException(nameof(marca),"Marca no puede ser null, estar vacias o solo contener espacios.");

            try
            {
                const string sqlString = 
                    "Insert Into ModeloVehiculo (NombreModelo, Marca, Version)" +
                    "Values (@nombreModelo, @marca, @version); Select Cast(SCOPE_IDENTITY() as int)";

                var id = DbCliente.GetConexion().Execute(sqlString, new {nombreModelo, marca, version});
                return new ModeloVehiculo(id, nombreModelo, marca, version);
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.Map(ex);
            }
        }

        public static List<ModeloVehiculo> GetModelos()
        {
            const string sqlString = "Select * From ModeloVehiculo";
            return DbCliente.GetConexion().Query<ModeloVehiculo>(sqlString).ToList();
        }

        public static ModeloVehiculo FindModelo(int modeloVehiculoId)
        {
            const string sqlString = "Select * From ModeloVehiculo Where ModeloVehiculo_ID = @modeloVehiculoId";
            return DbCliente.GetConexion().QueryFirstOrDefault<ModeloVehiculo>(sqlString, new {modeloVehiculoId});
        }

        public override string ToString()
        {
            return $"Modelo Vehiculo: [ID: {ModeloVehiculo_ID} / Nombre del Modelo: {NombreModelo} / " +
                   $"Marca: {Marca} / Version: {Version}]";
        }

    }
}