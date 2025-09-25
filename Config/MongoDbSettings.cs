/**
 * EVChargingWebService - MongoDbSettings
 * Defines configuration settings for connecting to MongoDB.
 * Author: EVChargingWebService
 * Date: 2025-09-25
 */

namespace EVChargingWebService.Config
{
    /// <summary>
    /// MongoDB connection settings bound from configuration.
    /// </summary>
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}


