namespace CareStack.Cache.UnifiedCache
{
    /// <summary>
    /// Enums For Different CacheTypes
    /// </summary>
    public enum UnifiedCacheStorageTypes
    {
        /// <summary>
        /// The memory cache
        /// </summary>
        MemCache,
        /// <summary>
        /// The redis cache
        /// </summary>
        RedisCache,
        /// <summary>
        /// From configuration
        /// </summary>
        FromConfig
    }
}