﻿using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            //aqui iria nuestra cadena de conecion
            string connectionString = "cache-coches.q0786q.ng.0001.use1.cache.amazonaws.com:6379";
            return ConnectionMultiplexer.Connect(connectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get 
            {
                return CreateConnection.Value;
            }
        }
    }
}
