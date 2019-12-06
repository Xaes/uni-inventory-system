using System;
using Domain.DB;
using Domain.Locations;

namespace Domain
{
    class Testing
    {
        
        private const string HOST = "localhost";
        private const string DBNAME = "inventorydb";
        private const string USER = "sa";
        private const string PASSWORD = "Password123!";
        
        public static void Main()
        {
            
            // Connecting to DB.
            Client.GetClient().Init(HOST, DBNAME, USER, PASSWORD);
            
            // Reading Data.
            Console.WriteLine(string.Join(Environment.NewLine,LocationManager.GetLocationManager().GetStorageList()));
            
            // Finding By ID.
            Console.WriteLine(LocationManager.GetLocationManager().FindStorage(1));
            
        }
    }
}

