using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.DB;

namespace Domain.Locations
{
    public class LocationManager
    {
        
        private static readonly LocationManager Single = new LocationManager();

        private LocationManager() {}

        public List<dynamic> GetStorageList()
        {
            return Client.GetConnection()
                .Query("Select * From Storage")
                .ToList();
        }

        public dynamic FindStorage(int id)
        {
            return Client.GetConnection()
                .Query("Select * From Storage Where Id = @id", new { id })
                .SingleOrDefault();
        }

        public static LocationManager GetLocationManager() { return Single; }
        
    }
}