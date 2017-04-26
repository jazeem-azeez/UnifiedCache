using StackExchange.Redis;
using System;

namespace UnifiedCache.Utility.Redis
{
    /// <summary>
    /// Factory For Redis Connection
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class RedisFactory : IDisposable
    {
        /// <summary>
        /// The lazy connection
        /// </summary>
        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
         {
             const string connectionString = "localhost";//System.Configuration.ConfigurationManager.AppSettings["RedisConnection"].ToString();

            var options = ConfigurationOptions.Parse(connectionString);
             options.AbortOnConnectFail = false;
             return ConnectionMultiplexer.Connect(options);
         });

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                GC.SuppressFinalize(this);
            }


        }
    }
}